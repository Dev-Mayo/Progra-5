using System;
using System.Web.UI;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.MasterPages
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.HaySesion())
            {
                Response.Redirect("~/Admin/SA1_Login.aspx?msg=nosesion");
                return;
            }

            if (SessionHelper.SesionExpirada())
            {
                SessionHelper.CerrarSesion();
                Response.Redirect("~/Admin/SA1_Login.aspx?msg=expirado");
                return;
            }

            if (SessionHelper.Rol != "ADMIN")
            {
                Response.Redirect("~/Admin/SA1_Login.aspx?msg=nosesion");
                return;
            }

            SessionHelper.RenovarActividad();
            lblUsuario.Text = SessionHelper.NombreCompleto;
            ConfigurarMenu();
        }

        private void ConfigurarMenu()
        {
            lnkInicio.Visible = true;
            lnkUsuarios.Visible = true;
            lnkRoles.Visible = true;
            lnkPantallas.Visible = true;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            SessionHelper.CerrarSesion();
            Response.Redirect("~/Admin/SA1_Login.aspx");
        }
    }
}