using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL7_ConsultaSaldo : System.Web.UI.Page
    {
        // SINGLETON
        private static readonly HttpClient _httpClient = new HttpClient();

       
        private class ApiResponse
        {
            public int codigo { get; set; }
            public string descripcion { get; set; }
        }

        
        private async Task<(bool ok, string mensaje, dynamic data)> LlamarApiAsync(string url, string token)
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

        
        private async Task<string> ObtenerIdentificacionAsync(string clienteId, string token)
        {
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/accounts/cliente-identificacion?clienteId={1}",
                            baseUrl.TrimEnd('/'),
                            Uri.EscapeDataString(clienteId));

            var (ok, _, data) = await LlamarApiAsync(url, token);
            if (!ok || data == null) return string.Empty;
            return (string)data ?? string.Empty;
        }

      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string telQS = Request.QueryString["telefono"];

                if (!string.IsNullOrEmpty(telQS))
                {
                    txtTelefono.Text = telQS;
                    RegisterAsyncTask(new PageAsyncTask(async () => await ConsultarSaldoAsync(telQS)));
                }
            }
        }

        
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string telefono = txtTelefono.Text.Trim();

            
            if (string.IsNullOrWhiteSpace(telefono))
            {
                MostrarMensaje("Debe enviar los datos completos y válidos", false);
                return;
            }

            
            if (telefono.Length != 8)
            {
                MostrarMensaje("El número de teléfono debe contener exactamente 8 dígitos.", false);
                return;
            }

            
            Response.Redirect(Request.Url.AbsolutePath + "?telefono=" + telefono);
        }

        
        private async Task ConsultarSaldoAsync(string telefono)
        {
            pnlMensaje.Visible = false;
            pnlResultado.Visible = false;

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

            // SRV13: GET /api/accounts/balance?telefono=...&identificacion=...
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/accounts/balance?telefono={1}&identificacion={2}",
                            baseUrl.TrimEnd('/'),
                            Uri.EscapeDataString(telefono),
                            Uri.EscapeDataString(identificacion));

            try
            {
                
                var (ok, mensaje, data) = await LlamarApiAsync(url, token);

                if (!ok)
                {
                    MostrarMensaje(mensaje, false);
                    return;
                }

                // 200 — mostramos los datos exitosamente
                lblNumeroCuenta.Text = (string)data["numeroCuenta"];
                lblSaldo.Text = ((decimal)data["saldo"]).ToString("N2");
                lblTelefono.Text = (string)data["telefono"];
                pnlResultado.Visible = true;

                // Mensaje "Consulta exitosa" (viene de la API)
                MostrarMensaje(mensaje, true);
            }
            catch (Exception)
            {
                MostrarMensaje("No fue posible consultar el saldo en este momento. Intente más tarde.", false);
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