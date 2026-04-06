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

            // Ocultar mensajes anteriores

            pnlMensaje.Visible = false;

            pnlBloqueado.Visible = false;

            string email = txtUsuario.Text.Trim();

            if (BloqueoService.EstaBlockeado(email))

            {

                pnlBloqueado.Visible = true;

                return;

            }

            // Recibir tupla del LoginAsync

            var (success, data, errorMessage) = await AuthService.LoginAsync(email, txtPassword.Text.Trim());

            if (success)

            {

                var datos = UsuarioService.ObtenerDatos(email);

                // Verificar que sea CLIENTE/USUARIO

                if (datos == null || datos.Rol != "CLIENTE/USUARIO")

                {

                    MostrarMensaje("Usuario y/o contraseña incorrectos.", "danger");

                    return;

                }

                BloqueoService.ResetearIntentos(email);

                Session["AccessToken"] = data.access_token;

                Session["UsuarioId"] = datos.Id;

                Session["NombreCompleto"] = datos.NombreCompleto;

                Session["Rol"] = "CLIENTE/USUARIO";

                Session["UltimaActividad"] = DateTime.Now;

                Response.Redirect("~/Portal/PTL2_Welcome.aspx", false);

            }

            else

            {

                bool bloqueado = BloqueoService.RegistrarIntentoFallido(email);

                if (bloqueado)

                {

                    pnlBloqueado.Visible = true;

                }

                else

                {

                    MostrarMensaje(errorMessage, "danger");

                }

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
