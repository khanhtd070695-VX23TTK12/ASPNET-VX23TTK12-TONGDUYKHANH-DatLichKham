using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DatLichKham.Admin
{
    public partial class QuanLyChuyenKhoa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ Admin được quản lý chuyên khoa
            if ((Session["VaiTro"] as string) != "Admin")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack) LoadData();
        }

        private void LoadData()
        {
            gvChuyenKhoa.DataSource = DatabaseHelper.GetTable(
                "SELECT MaCK, TenCK, MoTa FROM ChuyenKhoa ORDER BY TenCK");
            gvChuyenKhoa.DataBind();
        }

        private void Thongbao(bool thanhCong, string noiDung)
        {
            lblThongBao.CssClass = thanhCong ? "success" : "error";
            lblThongBao.Text = Server.HtmlEncode(noiDung);
        }

        private static bool LaLoiTrung(SqlException ex)
        {
            return ex.Number == 2627 || ex.Number == 2601;
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            string ten = txtTen.Text.Trim();
            if (ten == "")
            {
                Thongbao(false, "Vui lòng nhập tên chuyên khoa.");
                return;
            }

            try
            {
                DatabaseHelper.Execute(
                    "INSERT INTO ChuyenKhoa (TenCK, MoTa) VALUES (@ten, @mt)",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@mt", string.IsNullOrWhiteSpace(txtMoTa.Text)
                        ? (object)DBNull.Value : txtMoTa.Text.Trim()));

                txtTen.Text = "";
                txtMoTa.Text = "";
                Thongbao(true, "Đã thêm chuyên khoa \"" + ten + "\".");
                LoadData();
            }
            catch (SqlException ex)
            {
                Thongbao(false, LaLoiTrung(ex) ? "Tên chuyên khoa đã tồn tại." : "Lỗi: " + ex.Message);
            }
        }

        protected void gvChuyenKhoa_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvChuyenKhoa.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gvChuyenKhoa_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvChuyenKhoa.EditIndex = -1;
            LoadData();
        }

        protected void gvChuyenKhoa_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvChuyenKhoa.DataKeys[e.RowIndex].Value;
            GridViewRow row = gvChuyenKhoa.Rows[e.RowIndex];
            string ten = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string moTa = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();

            if (ten == "") { Thongbao(false, "Tên chuyên khoa không được để trống."); return; }
            if (ten.Length > 100) { Thongbao(false, "Tên chuyên khoa tối đa 100 ký tự."); return; }
            if (moTa.Length > 255) { Thongbao(false, "Mô tả tối đa 255 ký tự."); return; }

            try
            {
                DatabaseHelper.Execute(
                    "UPDATE ChuyenKhoa SET TenCK = @ten, MoTa = @mt WHERE MaCK = @id",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@mt", moTa == "" ? (object)DBNull.Value : moTa),
                    new SqlParameter("@id", id));

                gvChuyenKhoa.EditIndex = -1;
                Thongbao(true, "Đã cập nhật chuyên khoa.");
                LoadData();
            }
            catch (SqlException ex)
            {
                Thongbao(false, LaLoiTrung(ex) ? "Tên chuyên khoa đã tồn tại." : "Lỗi: " + ex.Message);
            }
        }

        protected void gvChuyenKhoa_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)gvChuyenKhoa.DataKeys[e.RowIndex].Value;
            try
            {
                DatabaseHelper.Execute("DELETE FROM ChuyenKhoa WHERE MaCK = @id",
                    new SqlParameter("@id", id));
                Thongbao(true, "Đã xóa chuyên khoa.");
            }
            catch (SqlException ex)
            {
                // 547 = vi phạm khóa ngoại (chuyên khoa đang có bác sĩ)
                Thongbao(false, ex.Number == 547
                    ? "Không thể xóa: chuyên khoa này đang có bác sĩ."
                    : "Lỗi: " + ex.Message);
            }
            gvChuyenKhoa.EditIndex = -1;
            LoadData();
        }
    }
}