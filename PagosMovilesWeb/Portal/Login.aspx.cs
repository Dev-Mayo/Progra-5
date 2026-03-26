using System;
using System.Web.UI;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class Login : Page
    {
        private const int MAX_INTENTOS = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SessionHelper.HaySesion() && !SessionHelper.SesionExpirada())
                {
                    Response.Redirect("~/Portal/PTL2_Welcome.aspx", false);
                    return;
                }
                switch (Request.QueryString["msg"])
                {
                    case "expirado":
                        MostrarMensaje(
                            "Su sesión expiró por inactividad. " +
                            "Por favor inicie sesión nuevamente.", "warning");
                        break;
                    case "nosesion":
                        MostrarMensaje(
                            "Por favor inicie sesión para utilizar el sistema.",
                            "info");
                        break;
                }
            }
        }

        protected async void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string email = txtUsuario.Text.Trim();

            // CAMBIO: Mostrar mensaje pero NO deshabilitar botón
            if (BloqueoService.EstaBlockeado(email))
            {
                pnlBloqueado.Visible = true;
                // REMOVIDO: btnIngresar.Enabled = false;
                return;
            }

            try
            {
                var resultado = await AuthService.LoginAsync(
                    email, txtPassword.Text.Trim());

                if (resultado != null)
                {
                    var datos = UsuarioService.ObtenerDatos(email);

                    // Verificar que sea CLIENTE/USUARIO
                    if (datos == null || datos.Rol != "CLIENTE/USUARIO")
                    {
                        MostrarMensaje(
                            "Usuario y/o contraseña incorrectos.", "danger");
                        return;
                    }

                    BloqueoService.ResetearIntentos(email);

                    Session["AccessToken"] = resultado.access_token;
                    Session["UsuarioId"] = datos.Id;
                    Session["NombreCompleto"] = datos.NombreCompleto;
                    Session["Rol"] = "CLIENTE/USUARIO";
                    Session["UltimaActividad"] = DateTime.Now;

                    Response.Redirect("~/Portal/PTL2_Welcome.aspx", false);
                }
                else
                {
                    bool bloqueado = BloqueoService.RegistrarIntentoFallido(email);

                    // ✅ CAMBIO: Mostrar mensaje pero NO deshabilitar botón
                    if (bloqueado)
                    {
                        pnlBloqueado.Visible = true;
                        // ❌ REMOVIDO: btnIngresar.Enabled = false;
                    }
                    else
                    {
                        MostrarMensaje(
                            "Usuario y/o contraseña incorrectos.", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "Error al conectar con el servicio. Intente de nuevo.",
                    "danger");
                System.Diagnostics.Debug.WriteLine(
                    $"[PTL Login Error] {ex.Message}");
            }
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            pnlMensaje.Visible = true;
            divMensaje.InnerText = texto;
            divMensaje.Attributes["class"] = $"alert alert-{tipo}";
        }
    }
}