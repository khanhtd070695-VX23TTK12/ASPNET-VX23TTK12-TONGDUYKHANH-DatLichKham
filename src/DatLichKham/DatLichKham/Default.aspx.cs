using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DatLichKham
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["QLDatLichKham"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM BenhNhan", con);
                    lblKetQua.Text = "Kết nối thành công! Số bệnh nhân: " + cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                lblKetQua.Text = "Lỗi kết nối: " + ex.Message;
            }
        }
    }
}