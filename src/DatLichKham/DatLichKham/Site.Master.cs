using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DatLichKham
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool daDangNhap = Session["MaTK"] != null;
            string vaiTro = Session["VaiTro"] as string;

            phKhach.Visible = !daDangNhap;
            phDaDangNhap.Visible = daDangNhap;
            phBenhNhan.Visible = vaiTro == "BenhNhan";
            phQuanTri.Visible = vaiTro == "Admin" || vaiTro == "LeTan";

            if (daDangNhap)
                lblXinChao.Text = "Xin chào, " + Server.HtmlEncode(Session["HoTen"] as string) + " ";
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }
    }
}