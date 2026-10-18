using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace DatLichKham.Admin
{
    public partial class QuanLyBacSi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ Admin được quản lý bác sĩ
            if ((Session["VaiTro"] as string) != "Admin")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack)
            {
                NapChuyenKhoa(ddlCKMoi, null);
                LoadData();
            }
        }

        private void NapChuyenKhoa(DropDownList ddl, string chon)
        {
            DataTable dt = DatabaseHelper.GetTable("SELECT MaCK, TenCK FROM ChuyenKhoa ORDER BY TenCK");
            ddl.Items.Clear();
            ddl.DataSource = dt;
            ddl.DataTextField = "TenCK";
            ddl.DataValueField = "MaCK";
            ddl.DataBind();
            if (chon != null && ddl.Items.FindByValue(chon) != null)
                ddl.SelectedValue = chon;
        }

        private void LoadData()
        {
            gvBacSi.DataSource = DatabaseHelper.GetTable(
                @"SELECT bs.MaBS, bs.HoTen, bs.MaCK, ck.TenCK, bs.HocVi, bs.SDT, bs.TrangThai
                  FROM BacSi bs JOIN ChuyenKhoa ck ON ck.MaCK = bs.MaCK
                  ORDER BY bs.HoTen");
            gvBacSi.DataBind();
        }

        private void Thongbao(bool thanhCong, string noiDung)
        {
            lblThongBao.CssClass = thanhCong ? "success" : "error";
            lblThongBao.Text = Server.HtmlEncode(noiDung);
        }

        // Trả về chuỗi báo lỗi, hoặc null nếu dữ liệu hợp lệ
        private static string KiemTra(string hoTen, string sdt, string maCK)
        {
            if (hoTen == "") return "Vui lòng nhập họ tên bác sĩ.";
            if (string.IsNullOrEmpty(maCK)) return "Vui lòng chọn chuyên khoa (hãy tạo chuyên khoa trước).";
            if (sdt != "" && !Regex.IsMatch(sdt, @"^0\d{9,10}$"))
                return "Số điện thoại phải bắt đầu bằng 0 và gồm 10-11 số.";
            return null;
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string loi = KiemTra(hoTen, sdt, ddlCKMoi.SelectedValue);
            if (loi != null) { Thongbao(false, loi); return; }

            try
            {
                DatabaseHelper.Execute(
                    "INSERT INTO BacSi (HoTen, MaCK, HocVi, SDT) VALUES (@ten, @ck, @hv, @sdt)",
                    new SqlParameter("@ten", hoTen),
                    new SqlParameter("@ck", int.Parse(ddlCKMoi.SelectedValue)),
                    new SqlParameter("@hv", txtHocVi.Text.Trim() == "" ? (object)DBNull.Value : txtHocVi.Text.Trim()),
                    new SqlParameter("@sdt", sdt == "" ? (object)DBNull.Value : sdt));

                txtHoTen.Text = ""; txtHocVi.Text = ""; txtSDT.Text = "";
                Thongbao(true, "Đã thêm bác sĩ \"" + hoTen + "\".");
                LoadData();
            }
            catch (SqlException ex)
            {
                Thongbao(false, "Lỗi: " + ex.Message);
            }
        }

        // Khi hàng đang ở chế độ sửa: nạp danh sách chuyên khoa và chọn đúng giá trị hiện tại
        protected void gvBacSi_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            if ((e.Row.RowState & DataControlRowState.Edit) == 0) return;

            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlCKSua");
            string maCK = ((DataRowView)e.Row.DataItem)["MaCK"].ToString();
            NapChuyenKhoa(ddl, maCK);
        }

        protected void gvBacSi_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBacSi.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gvBacSi_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBacSi.EditIndex = -1;
            LoadData();
        }

        protected void gvBacSi_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvBacSi.DataKeys[e.RowIndex].Value;
            GridViewRow row = gvBacSi.Rows[e.RowIndex];

            string hoTen = ((TextBox)row.FindControl("txtHoTenSua")).Text.Trim();
            string maCK = ((DropDownList)row.FindControl("ddlCKSua")).SelectedValue;
            string hocVi = ((TextBox)row.FindControl("txtHocViSua")).Text.Trim();
            string sdt = ((TextBox)row.FindControl("txtSDTSua")).Text.Trim();
            bool trangThai = ((CheckBox)row.FindControl("chkTrangThaiSua")).Checked;

            string loi = KiemTra(hoTen, sdt, maCK);
            if (loi != null) { Thongbao(false, loi); return; }

            try
            {
                DatabaseHelper.Execute(
                    @"UPDATE BacSi SET HoTen = @ten, MaCK = @ck, HocVi = @hv, SDT = @sdt, TrangThai = @tt
                      WHERE MaBS = @id",
                    new SqlParameter("@ten", hoTen),
                    new SqlParameter("@ck", int.Parse(maCK)),
                    new SqlParameter("@hv", hocVi == "" ? (object)DBNull.Value : hocVi),
                    new SqlParameter("@sdt", sdt == "" ? (object)DBNull.Value : sdt),
                    new SqlParameter("@tt", trangThai),
                    new SqlParameter("@id", id));

                gvBacSi.EditIndex = -1;
                Thongbao(true, "Đã cập nhật thông tin bác sĩ.");
                LoadData();
            }
            catch (SqlException ex)
            {
                Thongbao(false, "Lỗi: " + ex.Message);
            }
        }

        protected void gvBacSi_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)gvBacSi.DataKeys[e.RowIndex].Value;
            try
            {
                DatabaseHelper.Execute("DELETE FROM BacSi WHERE MaBS = @id", new SqlParameter("@id", id));
                Thongbao(true, "Đã xóa bác sĩ.");
            }
            catch (SqlException ex)
            {
                // 547 = vi phạm khóa ngoại (bác sĩ đã có khung giờ khám)
                Thongbao(false, ex.Number == 547
                    ? "Không thể xóa: bác sĩ đã có khung giờ khám. Hãy sửa trạng thái thành Ngừng làm việc."
                    : "Lỗi: " + ex.Message);
            }
            gvBacSi.EditIndex = -1;
            LoadData();
        }
    }
}