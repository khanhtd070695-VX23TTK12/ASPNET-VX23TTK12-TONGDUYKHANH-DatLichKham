using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DatLichKham
{
    public partial class LichCuaToi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ bệnh nhân đã đăng nhập mới xem được
            if ((Session["VaiTro"] as string) != "BenhNhan")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack) LoadLich();
        }

        private void LoadLich()
        {
            DataTable dt = DatabaseHelper.GetTable(
                @"SELECT MaLH, TenBacSi, TenCK, NgayKham,
                         LEFT(CONVERT(VARCHAR(8), GioBatDau, 108), 5) + ' - ' +
                         LEFT(CONVERT(VARCHAR(8), GioKetThuc, 108), 5) AS KhungGio,
                         LyDoKham, TrangThai
                  FROM vw_LichHenChiTiet
                  WHERE MaBN = @bn
                  ORDER BY NgayKham DESC, GioBatDau DESC",
                new SqlParameter("@bn", (int)Session["MaBN"]));
            gvLich.DataSource = dt;
            gvLich.DataBind();
        }

        // Chỉ hiện nút Hủy với lịch còn hiệu lực
        protected void gvLich_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            string tt = drv["TrangThai"].ToString();
            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkHuy");
            lnk.Visible = (tt == "Chờ xác nhận" || tt == "Đã xác nhận");
        }

        protected void gvLich_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Huy") return;

            int maLH = int.Parse(e.CommandArgument.ToString());
            try
            {
                DatabaseHelper.Execute("EXEC sp_HuyLich @lh, @bn",
                    new SqlParameter("@lh", maLH),
                    new SqlParameter("@bn", (int)Session["MaBN"]));

                lblThongBao.CssClass = "success";
                lblThongBao.Text = "Đã hủy lịch hẹn mã " + maLH + ".";
            }
            catch (SqlException ex)
            {
                lblThongBao.CssClass = "error";
                lblThongBao.Text = Server.HtmlEncode(ex.Message);
            }
            LoadLich();
        }
    }
}