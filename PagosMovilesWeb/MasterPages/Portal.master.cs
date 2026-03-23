using System;
using System.Web.UI;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.MasterPages
{
    public partial class Portal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.HaySesion())
            {
                Response.Redirect("~/Portal/Login.aspx?msg=nosesion");
                return;
            }

            if (SessionHelper.SesionExpirada())
            {
                SessionHelper.CerrarSesion();
                Response.Redirect("~/Portal/Login.aspx?msg=expirado");
                return;
            }

            if (SessionHelper.Rol != "CLIENTE/USUARIO")
            {
                Response.Redirect("~/Portal/Login.aspx?msg=nosesion");
                return;
            }

            SessionHelper.RenovarActividad();
            lblUsuarioPortal.Text = SessionHelper.NombreCompleto;
            ConfigurarMenu();
        }

        private void ConfigurarMenu()
        {
            lnkInicio.Visible = true;
            lnkDesinscribir.Visible = true;   // PTL6
            lnkSaldo.Visible = true;
            lnkMovimientos.Visible = true;
            lnkTransferir.Visible = true;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            SessionHelper.CerrarSesion();
            Response.Redirect("~/Portal/Login.aspx");
        }
    }
}