using System;
using System.Web.UI;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class Bienvenida : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // Verificar sesión
            if (!SessionHelper.HaySesion())
            {
                Response.Redirect("~/Portal/Login.aspx?msg=nosesion");
                return;
            }

            // Verificar inactividad
            if (SessionHelper.SesionExpirada())
            {
                SessionHelper.CerrarSesion();
                Response.Redirect("~/Portal/Login.aspx?msg=expirado");
                return;
            }

            // Validar token contra API
            bool tokenValido = await AuthService.ValidateTokenAsync(
                SessionHelper.AccessToken);

            if (!tokenValido)
            {
                SessionHelper.CerrarSesion();
                Response.Redirect("~/Portal/Login.aspx?msg=expirado");
                return;
            }

            // Renovar actividad
            SessionHelper.RenovarActividad();

            if (!IsPostBack)
            {
                lblNombre.Text = SessionHelper.NombreCompleto;
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            SessionHelper.CerrarSesion();
            Response.Redirect("~/Portal/Login.aspx");
        }
    }
}