using DatLichKham.Helpers;
using System;

namespace DatLichKham
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int bs = Convert.ToInt32(DatabaseHelper.Scalar("SELECT COUNT(*) FROM BacSi WHERE TrangThai = 1"));
            int ck = Convert.ToInt32(DatabaseHelper.Scalar("SELECT COUNT(*) FROM ChuyenKhoa"));
            lblThongTin.Text = "Hiện có " + bs + " bác sĩ thuộc " + ck + " chuyên khoa.";
        }
    }
}