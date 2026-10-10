using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DatLichKham
{
    public partial class DangNhap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            DataTable dt = DatabaseHelper.GetTable(
                @"SELECT tk.MaTK, tk.TenDangNhap, vt.TenVaiTro, tk.TrangThai
                  FROM TaiKhoan tk JOIN VaiTro vt ON vt.MaVaiTro = tk.MaVaiTro
                  WHERE tk.TenDangNhap = @u AND tk.MatKhauHash = @h",
                new SqlParameter("@u", txtTenDangNhap.Text.Trim()),
                new SqlParameter("@h", DatabaseHelper.HashPassword(txtMatKhau.Text)));

            if (dt.Rows.Count == 0)
            {
                lblThongBao.Text = "Sai tên đăng nhập hoặc mật khẩu.";
                return;
            }

            DataRow r = dt.Rows[0];
            if (!(bool)r["TrangThai"])
            {
                lblThongBao.Text = "Tài khoản đã bị khóa.";
                return;
            }

            int maTK = (int)r["MaTK"];
            string vaiTro = r["TenVaiTro"].ToString();
            string hoTen = r["TenDangNhap"].ToString();

            Session.Clear();
            if (vaiTro == "BenhNhan")
            {
                DataTable bn = DatabaseHelper.GetTable(
                    "SELECT MaBN, HoTen FROM BenhNhan WHERE MaTK = @tk",
                    new SqlParameter("@tk", maTK));
                if (bn.Rows.Count > 0)
                {
                    Session["MaBN"] = (int)bn.Rows[0]["MaBN"];
                    hoTen = bn.Rows[0]["HoTen"].ToString();
                }
            }
            Session["MaTK"] = maTK;
            Session["VaiTro"] = vaiTro;
            Session["HoTen"] = hoTen;

            Response.Redirect("~/Default.aspx");
        }
    }
}