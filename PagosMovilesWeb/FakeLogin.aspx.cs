using System;

namespace PagosMovilesWeb
{
    public partial class FakeLogin : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Session["UsuarioId"] = "123456789";
            Session["NombreCompleto"] = txtNombre.Text;
            Session["Rol"] = ddlRol.SelectedValue;
            Session["AccessToken"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Imp1YW4ucGVyZXpAbWFpbC5jb20iLCJpZCI6IjEyMzQ1Njc4OSIsIm5iZiI6MTc3MzYyNjE3OSwiZXhwIjoxNzczNjI2NDc5LCJpYXQiOjE3NzM2MjYxNzksImlzcyI6IlR1QXBwQXV0aCIsImF1ZCI6IlR1QXBwVXN1YXJpb3MifQ.E86c9HotKaal1tp7pAtNzKqf0mBxJzwoWQp_3Zfb7X8";

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