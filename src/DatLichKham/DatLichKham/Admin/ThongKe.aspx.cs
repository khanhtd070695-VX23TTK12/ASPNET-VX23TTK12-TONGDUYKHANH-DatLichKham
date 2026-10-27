using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DatLichKham.Admin
{
    public partial class ThongKe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ Admin được xem thống kê
            if ((Session["VaiTro"] as string) != "Admin")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack)
            {
                NapNam();
                LoadAll();
            }
        }

        private void NapNam()
        {
            DataTable dt = DatabaseHelper.GetTable(
                "SELECT DISTINCT YEAR(NgayKham) AS Nam FROM KhungGio UNION SELECT YEAR(GETDATE()) ORDER BY Nam DESC");
            ddlNam.DataSource = dt;
            ddlNam.DataTextField = "Nam";
            ddlNam.DataValueField = "Nam";
            ddlNam.DataBind();

            string nayNay = DateTime.Today.Year.ToString();
            if (ddlNam.Items.FindByValue(nayNay) != null) ddlNam.SelectedValue = nayNay;
        }

        protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAll();
        }

        // Thêm cột TyLe (0-100) so với giá trị lớn nhất, dùng để vẽ thanh biểu đồ
        private static void ThemTyLe(DataTable dt, string cot)
        {
            dt.Columns.Add("TyLe", typeof(int));
            int max = 0;
            foreach (DataRow r in dt.Rows)
                max = Math.Max(max, Convert.ToInt32(r[cot]));
            foreach (DataRow r in dt.Rows)
                r["TyLe"] = max > 0 ? Convert.ToInt32(r[cot]) * 100 / max : 0;
        }

        private void LoadAll()
        {
            SqlParameter nam = new SqlParameter("@nam", int.Parse(ddlNam.SelectedValue));

            // ---- Số liệu theo trạng thái (cũng dùng cho các ô tổng hợp) ----
            DataTable tt = DatabaseHelper.GetTable(
                @"SELECT TrangThai, COUNT(*) AS SoLich FROM vw_LichHenChiTiet
                  WHERE YEAR(NgayKham) = @nam GROUP BY TrangThai",
                new SqlParameter("@nam", int.Parse(ddlNam.SelectedValue)));

            string[] dsTrangThai = { "Chờ xác nhận", "Đã xác nhận", "Đã khám", "Đã hủy" };
            DataTable bangTT = new DataTable();
            bangTT.Columns.Add("TrangThai", typeof(string));
            bangTT.Columns.Add("SoLich", typeof(int));
            int tong = 0;
            foreach (string ten in dsTrangThai)
            {
                int dem = 0;
                foreach (DataRow r in tt.Rows)
                    if (r["TrangThai"].ToString() == ten) dem = Convert.ToInt32(r["SoLich"]);
                bangTT.Rows.Add(ten, dem);
                tong += dem;
            }
            ThemTyLe(bangTT, "SoLich");
            gvTrangThai.DataSource = bangTT;
            gvTrangThai.DataBind();

            lblTong.Text = tong.ToString();
            lblCho.Text = bangTT.Rows[0]["SoLich"].ToString();
            lblXacNhan.Text = bangTT.Rows[1]["SoLich"].ToString();
            lblDaKham.Text = bangTT.Rows[2]["SoLich"].ToString();
            lblDaHuy.Text = bangTT.Rows[3]["SoLich"].ToString();
            lblBenhNhan.Text = DatabaseHelper.Scalar("SELECT COUNT(*) FROM BenhNhan").ToString();

            // ---- Theo tháng: gọi thủ tục, bổ sung các tháng không có lịch để đủ 12 tháng ----
            DataTable kq = DatabaseHelper.GetTable("EXEC sp_ThongKeTheoThang @nam",
                new SqlParameter("@nam", int.Parse(ddlNam.SelectedValue)));
            DataTable bangThang = new DataTable();
            bangThang.Columns.Add("Thang", typeof(string));
            bangThang.Columns.Add("TongLich", typeof(int));
            bangThang.Columns.Add("DaKham", typeof(int));
            bangThang.Columns.Add("DaHuy", typeof(int));
            for (int m = 1; m <= 12; m++)
            {
                int a = 0, b = 0, c = 0;
                foreach (DataRow r in kq.Rows)
                {
                    if (Convert.ToInt32(r["Thang"]) == m)
                    {
                        a = Convert.ToInt32(r["TongLich"]);
                        b = Convert.ToInt32(r["DaKham"]);
                        c = Convert.ToInt32(r["DaHuy"]);
                    }
                }
                bangThang.Rows.Add("Tháng " + m, a, b, c);
            }
            ThemTyLe(bangThang, "TongLich");
            gvThang.DataSource = bangThang;
            gvThang.DataBind();

            // ---- Theo bác sĩ ----
            DataTable bs = DatabaseHelper.GetTable(
                @"SELECT TenBacSi, TenCK, COUNT(*) AS SoLich FROM vw_LichHenChiTiet
                  WHERE YEAR(NgayKham) = @nam AND TrangThai <> N'Đã hủy'
                  GROUP BY MaBS, TenBacSi, TenCK ORDER BY COUNT(*) DESC, TenBacSi",
                new SqlParameter("@nam", int.Parse(ddlNam.SelectedValue)));
            ThemTyLe(bs, "SoLich");
            gvBacSi.DataSource = bs;
            gvBacSi.DataBind();

            // ---- Theo chuyên khoa ----
            DataTable ck = DatabaseHelper.GetTable(
                @"SELECT TenCK, COUNT(*) AS SoLich FROM vw_LichHenChiTiet
                  WHERE YEAR(NgayKham) = @nam AND TrangThai <> N'Đã hủy'
                  GROUP BY TenCK ORDER BY COUNT(*) DESC, TenCK",
                new SqlParameter("@nam", int.Parse(ddlNam.SelectedValue)));
            ThemTyLe(ck, "SoLich");
            gvChuyenKhoa.DataSource = ck;
            gvChuyenKhoa.DataBind();
        }
    }
}