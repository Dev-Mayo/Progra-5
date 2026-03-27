using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL6_Desinscripcion : System.Web.UI.Page
    {
        //  SINGLETON 
        private static readonly HttpClientHandler _handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
        };
        private static readonly HttpClient _httpClient = new HttpClient(_handler);

        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE/USUARIO
        }

        // Obtiene la identificación llamando al API 
        private async Task<string> ObtenerIdentificacionAsync(string clienteId, string token)
        {
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/accounts/cliente-identificacion?clienteId={1}",
                            baseUrl.TrimEnd('/'),
                            Uri.EscapeDataString(clienteId));

            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            string body = await response.Content.ReadAsStringAsync();
            dynamic resp = JsonConvert.DeserializeObject(body);
            return (string)resp["data"] ?? string.Empty;
        }


        protected void btnDesinscribir_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(DesinscribirAsync));
        }

        private async Task DesinscribirAsync()
        {
            pnlMensaje.Visible = false;
            pnlMensaje.CssClass = "alert alert-danger shadow-sm mb-4";

            string telefono = txtTelefono.Text.Trim();
            string cuenta = txtCuenta.Text.Trim();

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

            // Capturar sesión ANTES del await
            string clienteId = SessionHelper.UsuarioId;
            string token = SessionHelper.AccessToken;

            if (string.IsNullOrEmpty(clienteId) || string.IsNullOrEmpty(token))
            {
                MostrarMensaje("Su sesión no es válida. Por favor inicie sesión nuevamente.", false);
                return;
            }

            // Obtener identificación desde el API 
            string identificacion = await ObtenerIdentificacionAsync(clienteId, token);
            if (string.IsNullOrEmpty(identificacion))
            {
                MostrarMensaje("Su sesión no es válida. Por favor inicie sesión nuevamente.", false);
                return;
            }

            // SRV10 del compañero Geancarlo
            string url = "https://localhost:7122/auth/cancel-subscription";

            var bodyObj = new
            {
                numeroCuenta = cuenta,
                identificacion = identificacion,
                numeroTelefono = telefono
            };
            string json = JsonConvert.SerializeObject(bodyObj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync(url, content);
                string body = await response.Content.ReadAsStringAsync();

                dynamic resp = JsonConvert.DeserializeObject(body);
                int codigo = (int)resp["codigo"];
                string descripcion = (string)resp["descripcion"];

                if (codigo == 0)
                {
                    MostrarMensaje(descripcion, true);
                    txtTelefono.Text = string.Empty;
                    txtCuenta.Text = string.Empty;
                }
                else
                {
                    MostrarMensaje(descripcion, false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("No fue posible procesar la desinscripción en este momento. Intente más tarde.", false);
            }
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = esExito
                ? "alert alert-success shadow-sm mb-4"
                : "alert alert-danger shadow-sm mb-4";
            lblMensaje.Text = mensaje;
        }
    }
}
