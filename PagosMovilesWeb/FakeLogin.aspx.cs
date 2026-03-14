using System;

namespace PagosMovilesWeb
{
    public partial class FakeLogin : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Session["UsuarioId"] = "1";
            Session["NombreCompleto"] = txtNombre.Text;
            Session["Rol"] = ddlRol.SelectedValue;
            Session["AccessToken"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im1hcmlhLmdvbWV6QG1haWwuY29tIiwiaWQiOiIxMDEwMDAwMDIiLCJuYmYiOjE3NzM0NzIwMjcsImV4cCI6MTc3MzQ3MjMyNywiaWF0IjoxNzczNDcyMDI3LCJpc3MiOiJUdUFwcEF1dGgiLCJhdWQiOiJUdUFwcFVzdWFyaW9zIn0.GTMacf3IdniTIpSPS9IR_jHfSn5hXZt2wue7ESmhA68";

            if (ddlRol.SelectedValue == "ADMIN")
            {
                Response.Redirect("~/Admin/SA2_Welcome.aspx");
            }
            else
            {
                Response.Redirect("~/Portal/PTL2_Welcome.aspx");
            }
        }
    }
}