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
    public partial class PTL7_ConsultaSaldo : System.Web.UI.Page
    {
        // SINGLETON
        private static readonly HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE/USUARIO
        }

        //  Obtiene la identificación llamando al API 
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

     
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(ConsultarSaldoAsync));
        }

        private async Task ConsultarSaldoAsync()
        {
            pnlMensaje.Visible = false;
            pnlResultado.Visible = false;

            string telefono = txtTelefono.Text.Trim();
            if (string.IsNullOrWhiteSpace(telefono) || telefono.Length != 8)
            {
                MostrarMensaje("El número de teléfono debe contener exactamente 8 dígitos.", false);
                return;
            }

           
            string clienteId = SessionHelper.UsuarioId;
            string token = SessionHelper.AccessToken;

            if (string.IsNullOrEmpty(clienteId) || string.IsNullOrEmpty(token))
            {
                MostrarMensaje("Su sesión no es válida. Por favor inicie sesión nuevamente.", false);
                return;
            }

            //  Obtener identificación desde el API 
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
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(url);
                string body = await response.Content.ReadAsStringAsync();

                dynamic resp = JsonConvert.DeserializeObject(body);
                int codigo = (int)resp["codigo"];
                string descripcion = (string)resp["descripcion"];

                if (codigo != 0)
                {
                    MostrarMensaje(descripcion, false);
                    return;
                }

                var data = resp["data"];
                lblNumeroCuenta.Text = (string)data["numeroCuenta"];
                lblSaldo.Text = ((decimal)data["saldo"]).ToString("N2");
                lblTelefono.Text = (string)data["telefono"];
                pnlResultado.Visible = true;
            }
            catch (Exception ex)
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
