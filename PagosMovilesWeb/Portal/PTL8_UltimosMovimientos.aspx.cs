using System;
using System.Collections.Generic;
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
    public partial class PTL8_UltimosMovimientos : System.Web.UI.Page
    {
        // SINGLETON
        private static readonly HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE/
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

        
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(ConsultarMovimientosAsync));
        }

        private async Task ConsultarMovimientosAsync()
        {
            pnlMensaje.Visible = false;
            pnlResultados.Visible = false;

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

            // SRV11: GET /api/accounts/transactions?telefono=...&identificacion=...
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/accounts/transactions?telefono={1}&identificacion={2}",
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
                lblTelefono.Text = (string)data["telefono"];

                var movimientos = JsonConvert.DeserializeObject<List<MovimientoVM>>(
                    data["movimientos"].ToString());

                gvMovimientos.DataSource = movimientos;
                gvMovimientos.DataBind();
                pnlResultados.Visible = true;
            }
            catch (Exception ex)
            {
                MostrarMensaje("No fue posible consultar los movimientos en este momento. Intente más tarde.", false);
            }
        }

        protected string FormatearTipo(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return tipo;
            if (tipo.ToUpper() == "CREDITO")
                return "<span style='color:green;font-weight:bold;'>▲ " + tipo + "</span>";
            return "<span style='color:red;font-weight:bold;'>▼ " + tipo + "</span>";
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

    public class MovimientoVM
    {
        public int movimientoId { get; set; }
        public string tipoMovimiento { get; set; }
        public decimal monto { get; set; }
        public decimal saldoAnterior { get; set; }
        public decimal saldoActual { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaMovimiento { get; set; }
    }
}

