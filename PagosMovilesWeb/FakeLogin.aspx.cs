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
            Session["AccessToken"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im1hcmlhLmdvbWV6QG1haWwuY29tIiwiaWQiOiIxMDEwMDAwMDIiLCJuYmYiOjE3NzM2MTU4NTIsImV4cCI6MTc3MzYxNjE1MiwiaWF0IjoxNzczNjE1ODUyLCJpc3MiOiJUdUFwcEF1dGgiLCJhdWQiOiJUdUFwcFVzdWFyaW9zIn0.XUKJpUys1u8k_nrQhX2Bdkv2S6Gd7gSuR2yC_cA7qF8";

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