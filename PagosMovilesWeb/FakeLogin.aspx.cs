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
            Session["AccessToken"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im1hcmlhLmdvbWV6QG1haWwuY29tIiwiaWQiOiIxMDEwMDAwMDIiLCJuYmYiOjE3NzMzODQ5NzgsImV4cCI6MTc3MzM4NTI3OCwiaWF0IjoxNzczMzg0OTc4LCJpc3MiOiJUdUFwcEF1dGgiLCJhdWQiOiJUdUFwcFVzdWFyaW9zIn0.VR2QV9qxEgsaaIIdBeZHgAM8yv8opmsBMnY0xEgkozg";

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