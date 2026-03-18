using System;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.MasterPages
{
    public partial class Portal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.HaySesion())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (SessionHelper.Rol != "CLIENTE")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            lblUsuarioPortal.Text = SessionHelper.NombreCompleto;

            ConfigurarMenu();
        }

        private void ConfigurarMenu()
        {
            lnkInicio.Visible = true;
            lnkDesinscribir.Visible = true;   // PTL6 - Desinscripción (Paola)
            lnkSaldo.Visible = true;   // PTL7 - Consultar saldo (Paola)
            lnkMovimientos.Visible = true;   // PTL8 - Últimos movimientos (Paola)
            lnkSaldo.Visible = true;
            lnkMovimientos.Visible = true;
            lnkTransferir.Visible = true;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            SessionHelper.CerrarSesion();
            Response.Redirect("~/Login.aspx");
        }
    }
}
