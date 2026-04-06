using System;
using System.Configuration;
using System.Net;
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
        // SINGLETON
        private static readonly HttpClientHandler _handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
        };
        private static readonly HttpClient _httpClient = new HttpClient(_handler);

        protected void Page_Load(object sender, EventArgs e)
        {
        }


        private class ApiResponse
        {
            public int codigo { get; set; }
            public string descripcion { get; set; }
        }

        // (GET): llama a la API y devuelve (ok, mensaje, data) 
        private async Task<(bool ok, string mensaje, dynamic data)> LlamarApiGetAsync(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            string body = await response.Content.ReadAsStringAsync();

            // 401 — sesión inválida o token expirado
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (false, "Su sesión no es válida. Por favor inicie sesión nuevamente.", null);

            var resp = JsonConvert.DeserializeObject<ApiResponse>(body);

            // 400 — datos incompletos o inválidos
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return (false, resp.descripcion, null);

            // 404 — cliente no encontrado
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (false, resp.descripcion, null);

            // 500 — error interno del servidor
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return (false, resp.descripcion, null);

            // 200 — éxito
            if (response.StatusCode == HttpStatusCode.OK)
            {
                dynamic fullResp = JsonConvert.DeserializeObject(body);
                return (true, resp.descripcion, fullResp["data"]);
            }

            return (false, resp.descripcion, null);
        }


        private async Task<(bool ok, string mensaje)> LlamarApiPostAsync(string url, string token, StringContent content)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(url, content);
            string body = await response.Content.ReadAsStringAsync();

            // 401 — sesión inválida o token expirado
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (false, "Su sesión no es válida. Por favor inicie sesión nuevamente.");

            var resp = JsonConvert.DeserializeObject<ApiResponse>(body);

            // 400 — datos incompletos o inválidos
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return (false, resp.descripcion);

            // 404 — cliente no encontrado
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (false, resp.descripcion);

            // 500 — error interno del servidor
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return (false, resp.descripcion);

            // 200 — éxito
            if (response.StatusCode == HttpStatusCode.OK)
                return (true, resp.descripcion);

            return (false, resp.descripcion);
        }


        private async Task<string> ObtenerIdentificacionAsync(string clienteId, string token)
        {
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/accounts/cliente-identificacion?clienteId={1}",
                            baseUrl.TrimEnd('/'),
                            Uri.EscapeDataString(clienteId));

            var (ok, _, data) = await LlamarApiGetAsync(url, token);
            if (!ok || data == null) return string.Empty;
            return (string)data ?? string.Empty;
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

            // SRV10 Geancarlo (puerto 7122)
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
                // ── Llamada (POST) 
                var (ok, mensaje) = await LlamarApiPostAsync(url, token, content);

                if (ok)
                {
                    MostrarMensaje(mensaje, true);
                    txtTelefono.Text = string.Empty;
                    txtCuenta.Text = string.Empty;
                }
                else
                {
                    MostrarMensaje(mensaje, false);
                }
            }
            catch (Exception)
            {
                // Solo si la API de Geancarlo está completamente apagada
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