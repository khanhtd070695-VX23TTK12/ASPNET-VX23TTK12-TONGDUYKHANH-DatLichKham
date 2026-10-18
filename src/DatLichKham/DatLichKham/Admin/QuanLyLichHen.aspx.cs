using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DatLichKham.Admin
{
    public partial class QuanLyLichHen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ Admin và Lễ tân được vào
            string vaiTro = Session["VaiTro"] as string;
            if (vaiTro != "Admin" && vaiTro != "LeTan")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack) LoadLich();
        }

        private void LoadLich()
        {
            // Tìm kiếm tương đối: LIKE %từ khóa% trên tên bệnh nhân, tên bác sĩ, SĐT, mã lịch
            string kw = "%" + txtTuKhoa.Text.Trim() + "%";
            DataTable dt = DatabaseHelper.GetTable(
                @"SELECT MaLH, TenBenhNhan, SDT, TenBacSi, TenCK, NgayKham,
                         LEFT(CONVERT(VARCHAR(8), GioBatDau, 108), 5) + ' - ' +
                         LEFT(CONVERT(VARCHAR(8), GioKetThuc, 108), 5) AS KhungGio,
                         LyDoKham, TrangThai
                  FROM vw_LichHenChiTiet
                  WHERE (TenBenhNhan LIKE @kw OR TenBacSi LIKE @kw OR SDT LIKE @kw
                         OR CAST(MaLH AS NVARCHAR(20)) LIKE @kw)
                    AND (@tt = N'' OR TrangThai = @tt)
                  ORDER BY NgayKham DESC, GioBatDau DESC",
                new SqlParameter("@kw", kw),
                new SqlParameter("@tt", SqlDbType.NVarChar, 20) { Value = ddlTrangThai.SelectedValue });
            gvLich.DataSource = dt;
            gvLich.DataBind();
        }

        protected void btnTim_Click(object sender, EventArgs e)
        {
            gvLich.PageIndex = 0;
            lblThongBao.Text = "";
            LoadLich();
        }

        protected void gvLich_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLich.PageIndex = e.NewPageIndex;
            LoadLich();
        }

        // Chỉ hiện nút phù hợp với trạng thái hiện tại
        protected void gvLich_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string tt = ((DataRowView)e.Row.DataItem)["TrangThai"].ToString();
            e.Row.FindControl("lnkXacNhan").Visible = (tt == "Chờ xác nhận");
            e.Row.FindControl("lnkDaKham").Visible = (tt == "Đã xác nhận");
            e.Row.FindControl("lnkTuChoi").Visible = (tt == "Chờ xác nhận" || tt == "Đã xác nhận");
        }

        protected void gvLich_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sql;
            string thanhCong;
            switch (e.CommandName)
            {
                case "XacNhan":
                    sql = "UPDATE LichHen SET TrangThai = N'Đã xác nhận' WHERE MaLH = @id AND TrangThai = N'Chờ xác nhận'";
                    thanhCong = "Đã xác nhận lịch hẹn mã ";
                    break;
                case "DaKham":
                    sql = "UPDATE LichHen SET TrangThai = N'Đã khám' WHERE MaLH = @id AND TrangThai = N'Đã xác nhận'";
                    thanhCong = "Đã đánh dấu đã khám cho lịch hẹn mã ";
                    break;
                case "TuChoi":
                    sql = "UPDATE LichHen SET TrangThai = N'Đã hủy' WHERE MaLH = @id AND TrangThai IN (N'Chờ xác nhận', N'Đã xác nhận')";
                    thanhCong = "Đã từ chối lịch hẹn mã ";
                    break;
                default:
                    return;
            }

            int maLH = int.Parse(e.CommandArgument.ToString());
            int dong = DatabaseHelper.Execute(sql, new SqlParameter("@id", maLH));

            if (dong > 0)
            {
                lblThongBao.CssClass = "success";
                lblThongBao.Text = thanhCong + maLH + ".";
            }
            else
            {
                lblThongBao.CssClass = "error";
                lblThongBao.Text = "Lịch hẹn mã " + maLH + " đã đổi trạng thái, không thể thực hiện thao tác này.";
            }
            LoadLich();
        }
    }
}