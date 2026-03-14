using System;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Admin
{
    public partial class SA2_Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.HaySesion())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (SessionHelper.Rol != "ADMIN")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            lblNombreCompleto.Text = SessionHelper.NombreCompleto;
        }
    }
}