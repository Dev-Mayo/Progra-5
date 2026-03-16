using System;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.MasterPages
{
    public partial class Admin : System.Web.UI.MasterPage
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

            lblUsuario.Text = SessionHelper.NombreCompleto;

            ConfigurarMenu();
        }

        private void ConfigurarMenu()
        {
            lnkInicio.Visible = true;
            lnkUsuarios.Visible = true;
            lnkRoles.Visible = true;
            lnkPantallas.Visible = true;
            lnkEntidades.Visible = true;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            SessionHelper.CerrarSesion();
            Response.Redirect("~/Login.aspx");
        }
    }
}