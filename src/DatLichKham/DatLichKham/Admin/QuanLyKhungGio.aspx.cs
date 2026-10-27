using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace DatLichKham.Admin
{
    public partial class QuanLyKhungGio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ Admin được quản lý khung giờ
            if ((Session["VaiTro"] as string) != "Admin")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack)
            {
                NapBacSi();
                LoadData();
            }
        }

        // Hàm định dạng dùng trong file .aspx
        protected string Gio(object o)
        {
            return ((TimeSpan)o).ToString(@"hh\:mm");
        }

        protected string Ngay(object o)
        {
            return ((DateTime)o).ToString("dd/MM/yyyy");
        }

        protected string NgayIso(object o)
        {
            return ((DateTime)o).ToString("yyyy-MM-dd");
        }

        private void NapBacSi()
        {
            DataTable dt = DatabaseHelper.GetTable(
                @"SELECT bs.MaBS, bs.HoTen + N' – ' + ck.TenCK AS Ten
                  FROM BacSi bs JOIN ChuyenKhoa ck ON ck.MaCK = bs.MaCK
                  WHERE bs.TrangThai = 1 ORDER BY bs.HoTen");

            ddlBacSiMoi.DataSource = dt;
            ddlBacSiMoi.DataTextField = "Ten";
            ddlBacSiMoi.DataValueField = "MaBS";
            ddlBacSiMoi.DataBind();

            ddlLocBacSi.DataSource = dt;
            ddlLocBacSi.DataTextField = "Ten";
            ddlLocBacSi.DataValueField = "MaBS";
            ddlLocBacSi.DataBind();
            ddlLocBacSi.Items.Insert(0, new ListItem("Tất cả bác sĩ", "0"));
        }

        private void LoadData()
        {
            object ngayLoc = DBNull.Value;
            DateTime d;
            if (DateTime.TryParseExact(txtLocNgay.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out d))
                ngayLoc = d;

            gvKhungGio.DataSource = DatabaseHelper.GetTable(
                @"SELECT kg.MaKG, kg.MaBS, bs.HoTen AS TenBacSi, kg.NgayKham, kg.GioBatDau, kg.GioKetThuc,
                         CASE WHEN EXISTS (SELECT 1 FROM LichHen lh
                                           WHERE lh.MaKG = kg.MaKG AND lh.TrangThai <> N'Đã hủy')
                              THEN N'Đã có lịch' ELSE N'Trống' END AS TinhTrang
                  FROM KhungGio kg JOIN BacSi bs ON bs.MaBS = kg.MaBS
                  WHERE (@bs = 0 OR kg.MaBS = @bs)
                    AND (@ngay IS NULL OR kg.NgayKham = @ngay)
                  ORDER BY kg.NgayKham, kg.GioBatDau, bs.HoTen",
                new SqlParameter("@bs", int.Parse(ddlLocBacSi.SelectedValue)),
                new SqlParameter("@ngay", SqlDbType.Date) { Value = ngayLoc });
            gvKhungGio.DataBind();
        }

        private void Thongbao(bool thanhCong, string noiDung)
        {
            lblThongBao.CssClass = thanhCong ? "success" : "error";
            lblThongBao.Text = Server.HtmlEncode(noiDung);
        }

        // Trả về chuỗi báo lỗi, hoặc null nếu hợp lệ
        private static string KiemTraGio(DateTime ngay, TimeSpan bd, TimeSpan kt)
        {
            if (ngay.Date < DateTime.Today)
                return "Không thể tạo khung giờ cho ngày đã qua.";
            if (kt <= bd)
                return "Giờ kết thúc phải sau giờ bắt đầu.";

            bool cuoiTuan = ngay.DayOfWeek == DayOfWeek.Saturday || ngay.DayOfWeek == DayOfWeek.Sunday;
            if (cuoiTuan)
            {
                if (bd < new TimeSpan(8, 0, 0) || kt > new TimeSpan(21, 0, 0))
                    return "Thứ 7, Chủ nhật chỉ nhận khung giờ trong khoảng 08:00 - 21:00.";
            }
            else
            {
                if (bd < new TimeSpan(17, 0, 0) || kt > new TimeSpan(21, 0, 0))
                    return "Ngày thường chỉ nhận khung giờ ngoài giờ hành chính: 17:00 - 21:00.";
            }
            return null;
        }

        // Có chồng lấn với khung giờ khác của cùng bác sĩ trong ngày không
        private static bool BiChong(int maBS, DateTime ngay, TimeSpan bd, TimeSpan kt, int boQuaMaKG)
        {
            int n = Convert.ToInt32(DatabaseHelper.Scalar(
                @"SELECT COUNT(*) FROM KhungGio
                  WHERE MaBS = @bs AND NgayKham = @ngay AND MaKG <> @id
                    AND GioBatDau < @kt AND GioKetThuc > @bd",
                new SqlParameter("@bs", maBS),
                new SqlParameter("@ngay", SqlDbType.Date) { Value = ngay },
                new SqlParameter("@id", boQuaMaKG),
                new SqlParameter("@bd", SqlDbType.Time) { Value = bd },
                new SqlParameter("@kt", SqlDbType.Time) { Value = kt }));
            return n > 0;
        }

        private static bool CoLichDangHieuLuc(int maKG)
        {
            return Convert.ToInt32(DatabaseHelper.Scalar(
                "SELECT COUNT(*) FROM LichHen WHERE MaKG = @id AND TrangThai <> N'Đã hủy'",
                new SqlParameter("@id", maKG))) > 0;
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            if (ddlBacSiMoi.Items.Count == 0)
            {
                Thongbao(false, "Chưa có bác sĩ đang làm việc để tạo khung giờ.");
                return;
            }

            DateTime ngay;
            TimeSpan bd, kt;
            if (!DateTime.TryParseExact(txtNgay.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out ngay))
            { Thongbao(false, "Vui lòng chọn ngày khám."); return; }
            if (!TimeSpan.TryParse(txtGioBD.Text, out bd) || !TimeSpan.TryParse(txtGioKT.Text, out kt))
            { Thongbao(false, "Vui lòng nhập giờ bắt đầu và giờ kết thúc."); return; }

            string loi = KiemTraGio(ngay, bd, kt);
            if (loi != null) { Thongbao(false, loi); return; }

            int maBS = int.Parse(ddlBacSiMoi.SelectedValue);
            if (BiChong(maBS, ngay, bd, kt, 0))
            { Thongbao(false, "Khung giờ bị trùng hoặc chồng lấn với khung giờ đã có của bác sĩ này."); return; }

            try
            {
                DatabaseHelper.Execute(
                    "INSERT INTO KhungGio (MaBS, NgayKham, GioBatDau, GioKetThuc) VALUES (@bs, @ngay, @bd, @kt)",
                    new SqlParameter("@bs", maBS),
                    new SqlParameter("@ngay", SqlDbType.Date) { Value = ngay },
                    new SqlParameter("@bd", SqlDbType.Time) { Value = bd },
                    new SqlParameter("@kt", SqlDbType.Time) { Value = kt });

                Thongbao(true, "Đã thêm khung giờ " + ngay.ToString("dd/MM/yyyy") + " " +
                               bd.ToString(@"hh\:mm") + " - " + kt.ToString(@"hh\:mm") + ".");
                LoadData();
            }
            catch (SqlException ex)
            {
                Thongbao(false, (ex.Number == 2627 || ex.Number == 2601)
                    ? "Khung giờ này đã tồn tại." : "Lỗi: " + ex.Message);
            }
        }

        protected void btnLoc_Click(object sender, EventArgs e)
        {
            gvKhungGio.EditIndex = -1;
            gvKhungGio.PageIndex = 0;
            lblThongBao.Text = "";
            LoadData();
        }

        protected void gvKhungGio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvKhungGio.EditIndex = -1;
            gvKhungGio.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void gvKhungGio_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int maKG = (int)gvKhungGio.DataKeys[e.NewEditIndex].Values["MaKG"];
            if (CoLichDangHieuLuc(maKG))
            {
                Thongbao(false, "Khung giờ đã có lịch hẹn, không thể sửa.");
                return;
            }
            gvKhungGio.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gvKhungGio_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvKhungGio.EditIndex = -1;
            LoadData();
        }

        protected void gvKhungGio_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int maKG = (int)gvKhungGio.DataKeys[e.RowIndex].Values["MaKG"];
            int maBS = (int)gvKhungGio.DataKeys[e.RowIndex].Values["MaBS"];
            GridViewRow row = gvKhungGio.Rows[e.RowIndex];

            DateTime ngay;
            TimeSpan bd, kt;
            if (!DateTime.TryParseExact(((TextBox)row.FindControl("txtNgaySua")).Text, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out ngay))
            { Thongbao(false, "Vui lòng chọn ngày khám."); return; }
            if (!TimeSpan.TryParse(((TextBox)row.FindControl("txtGioBDSua")).Text, out bd) ||
                !TimeSpan.TryParse(((TextBox)row.FindControl("txtGioKTSua")).Text, out kt))
            { Thongbao(false, "Vui lòng nhập giờ bắt đầu và giờ kết thúc."); return; }

            string loi = KiemTraGio(ngay, bd, kt);
            if (loi != null) { Thongbao(false, loi); return; }

            if (CoLichDangHieuLuc(maKG))
            { Thongbao(false, "Khung giờ đã có lịch hẹn, không thể sửa."); return; }
            if (BiChong(maBS, ngay, bd, kt, maKG))
            { Thongbao(false, "Khung giờ bị trùng hoặc chồng lấn với khung giờ khác của bác sĩ này."); return; }

            try
            {
                DatabaseHelper.Execute(
                    "UPDATE KhungGio SET NgayKham = @ngay, GioBatDau = @bd, GioKetThuc = @kt WHERE MaKG = @id",
                    new SqlParameter("@ngay", SqlDbType.Date) { Value = ngay },
                    new SqlParameter("@bd", SqlDbType.Time) { Value = bd },
                    new SqlParameter("@kt", SqlDbType.Time) { Value = kt },
                    new SqlParameter("@id", maKG));

                gvKhungGio.EditIndex = -1;
                Thongbao(true, "Đã cập nhật khung giờ.");
                LoadData();
            }
            catch (SqlException ex)
            {
                Thongbao(false, (ex.Number == 2627 || ex.Number == 2601)
                    ? "Khung giờ này đã tồn tại." : "Lỗi: " + ex.Message);
            }
        }

        protected void gvKhungGio_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int maKG = (int)gvKhungGio.DataKeys[e.RowIndex].Values["MaKG"];
            try
            {
                DatabaseHelper.Execute("DELETE FROM KhungGio WHERE MaKG = @id", new SqlParameter("@id", maKG));
                Thongbao(true, "Đã xóa khung giờ.");
            }
            catch (SqlException ex)
            {
                // 547 = vi phạm khóa ngoại (khung giờ đã từng có lịch hẹn, kể cả lịch đã hủy)
                Thongbao(false, ex.Number == 547
                    ? "Không thể xóa: khung giờ này đã có lịch hẹn (kể cả lịch đã hủy)."
                    : "Lỗi: " + ex.Message);
            }
            gvKhungGio.EditIndex = -1;
            LoadData();
        }
    }
}