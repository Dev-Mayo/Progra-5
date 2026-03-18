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
            Session["AccessToken"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Imp1YW5AZW1haWwuY29tIiwiaWQiOiIxMDEwMDAwMDEiLCJuYmYiOjE3NzM2Mjk4NzgsImV4cCI6MTc3MzYzMDE3OCwiaWF0IjoxNzczNjI5ODc4LCJpc3MiOiJUdUFwcEF1dGgiLCJhdWQiOiJUdUFwcFVzdWFyaW9zIn0.1IIxn5YlNrOTwnzDSMrbxhrRv86Akxj75I3BogTfN6E";

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