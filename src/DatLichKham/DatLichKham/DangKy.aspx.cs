using DatLichKham.Helpers;
using System;
using System.Data.SqlClient;
using System.Globalization;

namespace DatLichKham
{
    public partial class DangKy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string user = txtTenDangNhap.Text.Trim();

            DateTime ngaySinh;
            if (!DateTime.TryParseExact(txtNgaySinh.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out ngaySinh) || ngaySinh > DateTime.Today)
            {
                BaoLoi("Ngày sinh không hợp lệ.");
                return;
            }

            int tonTai = Convert.ToInt32(DatabaseHelper.Scalar(
                "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @u",
                new SqlParameter("@u", user)));
            if (tonTai > 0)
            {
                BaoLoi("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
                return;
            }

            string sql = @"SET XACT_ABORT ON;
                BEGIN TRAN;
                INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, MaVaiTro) VALUES (@u, @h, 3);
                DECLARE @tk INT = SCOPE_IDENTITY();
                INSERT INTO BenhNhan (MaTK, HoTen, NgaySinh, GioiTinh, SDT, DiaChi)
                VALUES (@tk, @ten, @ns, @gt, @sdt, @dc);
                COMMIT;";

            try
            {
                DatabaseHelper.Execute(sql,
                    new SqlParameter("@u", user),
                    new SqlParameter("@h", DatabaseHelper.HashPassword(txtMatKhau.Text)),
                    new SqlParameter("@ten", txtHoTen.Text.Trim()),
                    new SqlParameter("@ns", ngaySinh),
                    new SqlParameter("@gt", ddlGioiTinh.SelectedValue),
                    new SqlParameter("@sdt", txtSDT.Text.Trim()),
                    new SqlParameter("@dc", string.IsNullOrWhiteSpace(txtDiaChi.Text)
                        ? (object)DBNull.Value : txtDiaChi.Text.Trim()));

                lblThongBao.CssClass = "success";
                lblThongBao.Text = "Đăng ký thành công! Bạn có thể <a href='DangNhap.aspx'>đăng nhập</a> ngay.";
            }
            catch (SqlException ex)
            {
                BaoLoi("Không thể đăng ký: " + ex.Message);
            }
        }

        private void BaoLoi(string noiDung)
        {
            lblThongBao.CssClass = "error";
            lblThongBao.Text = Server.HtmlEncode(noiDung);
        }
    }
}