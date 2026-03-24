using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Configuration;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL6_Desinscripcion : System.Web.UI.Page
    {
        // ✅ SINGLETON correcto — un solo HttpClient para toda la aplicación
        // PTL6 usa HTTPS del compañero — HttpClientHandler para ignorar SSL local
        private static readonly HttpClientHandler _handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
        };
        private static readonly HttpClient _httpClient = new HttpClient(_handler);

        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE/USUARIO
        }

        // ✅ Convierte el cliente_id numérico a la cédula real
        private string ObtenerIdentificacionPorClienteId(string clienteId)
        {
            try
            {
                string connStr = WebConfigurationManager
                                    .ConnectionStrings["CoreBancario"].ConnectionString;
                using (var cn = new SqlConnection(connStr))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(
                        "SELECT identificacion FROM cliente WHERE cliente_id = @id", cn))
                    {
                        cmd.Parameters.AddWithValue("@id", clienteId);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? string.Empty;
                    }
                }
            }
            catch { return string.Empty; }
        }

        // ✅ async void — funciona con Async="true" en el .aspx, sin deadlock
        protected async void btnDesinscribir_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible  = false;
            pnlMensaje.CssClass = "alert alert-danger shadow-sm mb-4";

            string telefono = txtTelefono.Text.Trim();
            string cuenta   = txtCuenta.Text.Trim();

            if (string.IsNullOrWhiteSpace(telefono) || telefono.Length != 8)
            {
                MostrarMensaje("El número de teléfono debe contener exactamente 8 dígitos.", false);
                return;
            }
            if (string.IsNullOrWhiteSpace(cuenta))
            {
                MostrarMensaje("Debe ingresar el número de cuenta a desasociar.", false);
                return;
            }

            string identificacion = ObtenerIdentificacionPorClienteId(SessionHelper.UsuarioId);
            if (string.IsNullOrEmpty(identificacion))
            {
                MostrarMensaje("Su sesión no es válida. Por favor inicie sesión nuevamente.", false);
                return;
            }

            // Capturar token antes del await
            string token = SessionHelper.AccessToken;
            string url   = "https://localhost:7122/auth/cancel-subscription";

            var bodyObj = new
            {
                numeroCuenta   = cuenta,
                identificacion = identificacion,
                numeroTelefono = telefono
            };

            string json    = JsonConvert.SerializeObject(bodyObj);
            var    content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // ✅ Configurar singleton con el token capturado
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                // ✅ await real — sin deadlock gracias a Async="true" en el .aspx
                var response = await _httpClient.PostAsync(url, content);
                string body  = await response.Content.ReadAsStringAsync();

                dynamic resp        = JsonConvert.DeserializeObject(body);
                int     codigo      = (int)(resp.codigo ?? resp.Codigo);
                string  descripcion = (string)(resp.descripcion ?? resp.Descripcion);

                if (codigo == 0)
                {
                    MostrarMensaje(descripcion, true);
                    txtTelefono.Text = string.Empty;
                    txtCuenta.Text   = string.Empty;
                }
                else
                {
                    MostrarMensaje(descripcion, false);
                }
            }
            catch (Exception)
            {
                MostrarMensaje("No fue posible procesar la desinscripción en este momento. Intente más tarde.", false);
            }
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            pnlMensaje.Visible  = true;
            pnlMensaje.CssClass = esExito
                ? "alert alert-success shadow-sm mb-4"
                : "alert alert-danger shadow-sm mb-4";
            lblMensaje.Text = mensaje;
        }
    }
}
