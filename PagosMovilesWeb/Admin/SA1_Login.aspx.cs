using System;
using System.Web.UI;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Admin
{
    public partial class SA1_Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SessionHelper.HaySesion() && !SessionHelper.SesionExpirada())
                {
                    Response.Redirect("~/Admin/SA2_Welcome.aspx");
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

            if (BloqueoService.EstaBlockeado(email))
            {
                pnlBloqueado.Visible = true;
                btnIngresar.Enabled = false;
                return;
            }

            try
            {
                var resultado = await AuthService.LoginAsync(
                    email, txtPassword.Text.Trim());

                if (resultado != null)
                {
                    var datos = UsuarioService.ObtenerDatos(email);

                    // Verificar que sea ADMIN
                    if (datos == null || datos.Rol != "ADMIN")
                    {
                        MostrarMensaje(
                            "Usuario y/o contraseña incorrectos.", "danger");
                        return;
                    }

                    BloqueoService.ResetearIntentos(email);

                    Session["AccessToken"] = resultado.access_token;
                    Session["UsuarioId"] = datos.Id;
                    Session["NombreCompleto"] = datos.NombreCompleto;
                    Session["Rol"] = "ADMIN";
                    Session["UltimaActividad"] = DateTime.Now;

                    Response.Redirect("~/Admin/SA2_Welcome.aspx", false);
                }
                else
                {
                    bool bloqueado = BloqueoService.RegistrarIntentoFallido(email);

                    if (bloqueado)
                    {
                        pnlBloqueado.Visible = true;
                        btnIngresar.Enabled = false;
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
                    $"[SA1 Login Error] {ex.Message}");
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