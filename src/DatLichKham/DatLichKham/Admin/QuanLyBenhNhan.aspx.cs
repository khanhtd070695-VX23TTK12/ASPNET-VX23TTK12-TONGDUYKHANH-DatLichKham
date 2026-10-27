using DatLichKham.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace DatLichKham.Admin
{
    public partial class QuanLyBenhNhan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Chỉ Admin được quản lý bệnh nhân
            if ((Session["VaiTro"] as string) != "Admin")
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }
            if (!IsPostBack) LoadData();
        }

        // Hàm định dạng dùng trong file .aspx
        protected string Ngay(object o)
        {
            return o is DateTime ? ((DateTime)o).ToString("dd/MM/yyyy") : "";
        }

        protected string NgayIso(object o)
        {
            return o is DateTime ? ((DateTime)o).ToString("yyyy-MM-dd") : "";
        }

        private void LoadData()
        {
            // Tìm kiếm tương đối: LIKE %từ khóa%
            string kw = "%" + txtTuKhoa.Text.Trim() + "%";
            gvBenhNhan.DataSource = DatabaseHelper.GetTable(
                @"SELECT bn.MaBN, bn.MaTK, tk.TenDangNhap, bn.HoTen, bn.NgaySinh, bn.GioiTinh,
                         bn.SDT, bn.DiaChi, tk.TrangThai,
                         (SELECT COUNT(*) FROM LichHen lh WHERE lh.MaBN = bn.MaBN) AS SoLich
                  FROM BenhNhan bn JOIN TaiKhoan tk ON tk.MaTK = bn.MaTK
                  WHERE bn.HoTen LIKE @kw OR bn.SDT LIKE @kw OR tk.TenDangNhap LIKE @kw
                        OR CAST(bn.MaBN AS NVARCHAR(20)) LIKE @kw
                  ORDER BY bn.HoTen",
                new SqlParameter("@kw", kw));
            gvBenhNhan.DataBind();
        }

        private void Thongbao(bool thanhCong, string noiDung)
        {
            lblThongBao.CssClass = thanhCong ? "success" : "error";
            lblThongBao.Text = Server.HtmlEncode(noiDung);
        }

        protected void btnTim_Click(object sender, EventArgs e)
        {
            gvBenhNhan.EditIndex = -1;
            gvBenhNhan.PageIndex = 0;
            lblThongBao.Text = "";
            LoadData();
        }

        protected void gvBenhNhan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBenhNhan.EditIndex = -1;
            gvBenhNhan.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void gvBenhNhan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if ((e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                // Chế độ sửa: chọn đúng giới tính hiện tại
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlGioiTinhSua");
                string gt = drv["GioiTinh"].ToString();
                if (ddl.Items.FindByValue(gt) != null) ddl.SelectedValue = gt;
            }
            else
            {
                // Chế độ xem: nút Khóa / Mở khóa theo trạng thái tài khoản
                LinkButton lnk = (LinkButton)e.Row.FindControl("lnkKhoa");
                lnk.Text = (bool)drv["TrangThai"] ? "Khóa" : "Mở khóa";
            }
        }

        protected void gvBenhNhan_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBenhNhan.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void gvBenhNhan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBenhNhan.EditIndex = -1;
            LoadData();
        }

        protected void gvBenhNhan_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int maBN = (int)gvBenhNhan.DataKeys[e.RowIndex].Values["MaBN"];
            GridViewRow row = gvBenhNhan.Rows[e.RowIndex];

            string hoTen = ((TextBox)row.FindControl("txtHoTenSua")).Text.Trim();
            string ngayText = ((TextBox)row.FindControl("txtNgaySinhSua")).Text;
            string gioiTinh = ((DropDownList)row.FindControl("ddlGioiTinhSua")).SelectedValue;
            string sdt = ((TextBox)row.FindControl("txtSDTSua")).Text.Trim();
            string diaChi = ((TextBox)row.FindControl("txtDiaChiSua")).Text.Trim();

            if (hoTen == "") { Thongbao(false, "Họ tên không được để trống."); return; }

            DateTime ngaySinh;
            if (!DateTime.TryParseExact(ngayText, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out ngaySinh) || ngaySinh > DateTime.Today)
            { Thongbao(false, "Ngày sinh không hợp lệ."); return; }

            if (!Regex.IsMatch(sdt, @"^0\d{9,10}$"))
            { Thongbao(false, "Số điện thoại phải bắt đầu bằng 0 và gồm 10-11 số."); return; }

            try
            {
                DatabaseHelper.Execute(
                    @"UPDATE BenhNhan SET HoTen = @ten, NgaySinh = @ns, GioiTinh = @gt, SDT = @sdt, DiaChi = @dc
                      WHERE MaBN = @id",
                    new SqlParameter("@ten", hoTen),
                    new SqlParameter("@ns", SqlDbType.Date) { Value = ngaySinh },
                    new SqlParameter("@gt", gioiTinh),
                    new SqlParameter("@sdt", sdt),
                    new SqlParameter("@dc", diaChi == "" ? (object)DBNull.Value : diaChi),
                    new SqlParameter("@id", maBN));

                gvBenhNhan.EditIndex = -1;
                Thongbao(true, "Đã cập nhật thông tin bệnh nhân.");
                LoadData();
            }
            catch (SqlException ex)
            {
                Thongbao(false, "Lỗi: " + ex.Message);
            }
        }

        protected void gvBenhNhan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "KhoaMo") return;

            int maTK = int.Parse(e.CommandArgument.ToString());
            DatabaseHelper.Execute(
                "UPDATE TaiKhoan SET TrangThai = CASE WHEN TrangThai = 1 THEN 0 ELSE 1 END WHERE MaTK = @tk",
                new SqlParameter("@tk", maTK));

            Thongbao(true, "Đã đổi trạng thái tài khoản.");
            LoadData();
        }

        protected void gvBenhNhan_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int maBN = (int)gvBenhNhan.DataKeys[e.RowIndex].Values["MaBN"];
            int maTK = (int)gvBenhNhan.DataKeys[e.RowIndex].Values["MaTK"];
            try
            {
                // Xóa bệnh nhân rồi tài khoản trong cùng một giao dịch
                DatabaseHelper.Execute(
                    @"SET XACT_ABORT ON;
                      BEGIN TRAN;
                      DELETE FROM BenhNhan WHERE MaBN = @bn;
                      DELETE FROM TaiKhoan WHERE MaTK = @tk;
                      COMMIT;",
                    new SqlParameter("@bn", maBN),
                    new SqlParameter("@tk", maTK));
                Thongbao(true, "Đã xóa bệnh nhân.");
            }
            catch (SqlException ex)
            {
                // 547 = vi phạm khóa ngoại (bệnh nhân đã có lịch hẹn)
                Thongbao(false, ex.Number == 547
                    ? "Không thể xóa: bệnh nhân đã có lịch hẹn. Hãy dùng Khóa tài khoản."
                    : "Lỗi: " + ex.Message);
            }
            gvBenhNhan.EditIndex = -1;
            LoadData();
        }
    }
}