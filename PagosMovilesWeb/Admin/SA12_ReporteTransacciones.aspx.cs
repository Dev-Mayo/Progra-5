using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;
using System.Web.UI;

namespace PagosMovilesWeb.Admin
{
    public partial class SA12_ReporteTransacciones : System.Web.UI.Page
    {
        // SINGLETON
        private static readonly HttpClient _httpClient = new HttpClient();

        // Clase que mapea la respuesta estándar de la API
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

            // 400 — fecha no enviada o inválida
            if (response.StatusCode == HttpStatusCode.BadRequest)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                string fechaQS = Request.QueryString["fecha"];

                if (!string.IsNullOrEmpty(fechaQS))
                {
                    
                    txtFecha.Text = fechaQS;
                    RegisterAsyncTask(new PageAsyncTask(async () => await ConsultarReporteAsync(fechaQS)));
                }
                else
                {
                    
                    txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                }
            }
 
        }

        
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            
            string fecha = txtFecha.Text.Trim();

            if (!DateTime.TryParse(fecha, out _))
            {
                MostrarMensaje("Por favor seleccione una fecha válida para generar el reporte.", false);
                return;
            }

            
            Response.Redirect(Request.Url.AbsolutePath + "?fecha=" + fecha);
        }

        // Consulta el reporte llamando a la API
        private async Task ConsultarReporteAsync(string fechaStr)
        {
            pnlMensaje.Visible = false;
            pnlResultados.Visible = false;

            if (!DateTime.TryParse(fechaStr, out DateTime fechaSeleccionada))
            {
                MostrarMensaje("Por favor seleccione una fecha válida para generar el reporte.", false);
                return;
            }

            string token = SessionHelper.AccessToken;
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/reports/transactions/daily?fecha={1}",
                            baseUrl.TrimEnd('/'),
                            fechaSeleccionada.ToString("yyyy-MM-dd"));

            try
            {
                
                var (ok, mensaje, data) = await LlamarApiAsync(url, token);

                if (!ok)
                {
                    MostrarMensaje(mensaje, false);
                    return;
                }

                // 200 
                var transacciones = JsonConvert.DeserializeObject<List<TransaccionReporteVM>>(
                    data["transacciones"].ToString());
                decimal totalDia = (decimal)data["totalMonto"];

                // Lista vacía — mostramos mensaje informativo 
                if (transacciones == null || transacciones.Count == 0)
                {
                    MostrarMensaje("No se encontraron transacciones para la fecha indicada.", false);
                    return;
                }

                // Hay datos — mostramos tabla y mensaje verde de éxito
                gvTransacciones.DataSource = transacciones;
                gvTransacciones.DataBind();

                lblFechaConsultada.Text = fechaSeleccionada.ToString("dd/MM/yyyy");
                lblTotalDia.Text = totalDia.ToString("N2");
                pnlResultados.Visible = true;

                // Mensaje verde: "Reporte generado exitosamente" (viene de la API)
                MostrarMensaje(mensaje, true);
            }
            catch (Exception)
            {
                MostrarMensaje("No fue posible generar el reporte en este momento. Intente más tarde.", false);
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

    public class TransaccionReporteVM
    {
        public DateTime fecha { get; set; }
        public string telefonoOrigen { get; set; }
        public string telefonoDestino { get; set; }
        public decimal monto { get; set; }
    }
}
