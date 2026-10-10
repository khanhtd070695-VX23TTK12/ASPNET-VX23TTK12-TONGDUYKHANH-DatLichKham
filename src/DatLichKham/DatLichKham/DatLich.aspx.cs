using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace DatLichKham
{
    public partial class DatLich : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ bệnh nhân đã đăng nhập mới được đặt lịch
            if ((Session["VaiTro"] as string) != "BenhNhan")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack)
            {
                DataTable dt = DatabaseHelper.GetTable("SELECT MaCK, TenCK FROM ChuyenKhoa ORDER BY TenCK");
                Bind(ddlChuyenKhoa, dt, "TenCK", "MaCK", "-- Chọn chuyên khoa --");
                LoadBacSi();
                LoadNgay();
                LoadGio();
            }
        }

        // Đổ dữ liệu vào DropDownList, luôn có dòng đầu là gợi ý chọn
        private void Bind(DropDownList ddl, DataTable dt, string text, string value, string placeholder)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(placeholder, ""));
            if (dt == null) return;
            foreach (DataRow r in dt.Rows)
                ddl.Items.Add(new ListItem(r[text].ToString(), r[value].ToString()));
        }

        private void LoadBacSi()
        {
            DataTable dt = null;
            if (ddlChuyenKhoa.SelectedValue != "")
                dt = DatabaseHelper.GetTable(
                    "SELECT MaBS, HoTen FROM BacSi WHERE MaCK = @ck AND TrangThai = 1 ORDER BY HoTen",
                    new SqlParameter("@ck", int.Parse(ddlChuyenKhoa.SelectedValue)));
            Bind(ddlBacSi, dt, "HoTen", "MaBS", "-- Chọn bác sĩ --");
        }

        // Chỉ liệt kê ngày còn ít nhất 1 khung giờ trống và chưa qua
        private void LoadNgay()
        {
            ddlNgay.Items.Clear();
            ddlNgay.Items.Add(new ListItem("-- Chọn ngày --", ""));
            if (ddlBacSi.SelectedValue == "") return;

            DataTable dt = DatabaseHelper.GetTable(
                @"SELECT DISTINCT kg.NgayKham FROM KhungGio kg
                  WHERE kg.MaBS = @bs
                    AND CAST(kg.NgayKham AS DATETIME) + CAST(kg.GioBatDau AS DATETIME) > GETDATE()
                    AND NOT EXISTS (SELECT 1 FROM LichHen lh
                                    WHERE lh.MaKG = kg.MaKG AND lh.TrangThai <> N'Đã hủy')
                  ORDER BY kg.NgayKham",
                new SqlParameter("@bs", int.Parse(ddlBacSi.SelectedValue)));

            foreach (DataRow r in dt.Rows)
            {
                DateTime d = (DateTime)r["NgayKham"];
                ddlNgay.Items.Add(new ListItem(d.ToString("dd/MM/yyyy"), d.ToString("yyyy-MM-dd")));
            }
        }

        private void LoadGio()
        {
            ddlKhungGio.Items.Clear();
            ddlKhungGio.Items.Add(new ListItem("-- Chọn khung giờ --", ""));
            if (ddlBacSi.SelectedValue == "" || ddlNgay.SelectedValue == "") return;

            DateTime ngay = DateTime.ParseExact(ddlNgay.SelectedValue, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DataTable dt = DatabaseHelper.GetTable(
                @"SELECT kg.MaKG, kg.GioBatDau, kg.GioKetThuc FROM KhungGio kg
                  WHERE kg.MaBS = @bs AND kg.NgayKham = @ngay
                    AND CAST(kg.NgayKham AS DATETIME) + CAST(kg.GioBatDau AS DATETIME) > GETDATE()
                    AND NOT EXISTS (SELECT 1 FROM LichHen lh
                                    WHERE lh.MaKG = kg.MaKG AND lh.TrangThai <> N'Đã hủy')
                  ORDER BY kg.GioBatDau",
                new SqlParameter("@bs", int.Parse(ddlBacSi.SelectedValue)),
                new SqlParameter("@ngay", SqlDbType.Date) { Value = ngay });

            foreach (DataRow r in dt.Rows)
            {
                string gio = ((TimeSpan)r["GioBatDau"]).ToString(@"hh\:mm") + " - " +
                             ((TimeSpan)r["GioKetThuc"]).ToString(@"hh\:mm");
                ddlKhungGio.Items.Add(new ListItem(gio, r["MaKG"].ToString()));
            }
        }

        protected void ddlChuyenKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBacSi();
            LoadNgay();
            LoadGio();
        }

        protected void ddlBacSi_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadNgay();
            LoadGio();
        }

        protected void ddlNgay_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGio();
        }

        protected void btnDatLich_Click(object sender, EventArgs e)
        {
            if (ddlKhungGio.SelectedValue == "")
            {
                BaoLoi("Vui lòng chọn đầy đủ chuyên khoa, bác sĩ, ngày và khung giờ.");
                return;
            }

            try
            {
                object maLH = DatabaseHelper.Scalar(
                    "EXEC sp_DatLich @bn, @kg, @ld",
                    new SqlParameter("@bn", (int)Session["MaBN"]),
                    new SqlParameter("@kg", int.Parse(ddlKhungGio.SelectedValue)),
                    new SqlParameter("@ld", string.IsNullOrWhiteSpace(txtLyDo.Text)
                        ? (object)DBNull.Value : txtLyDo.Text.Trim()));

                lblThongBao.CssClass = "success";
                lblThongBao.Text = "Đặt lịch thành công! Mã lịch hẹn: " + maLH +
                                   ". Trạng thái: Chờ xác nhận.";
                txtLyDo.Text = "";
                LoadNgay();
                LoadGio();
            }
            catch (SqlException ex)
            {
                BaoLoi(ex.Message);
                LoadNgay();
                LoadGio();
            }
        }

        private void BaoLoi(string noiDung)
        {
            lblThongBao.CssClass = "error";
            lblThongBao.Text = Server.HtmlEncode(noiDung);
        }
    }
}