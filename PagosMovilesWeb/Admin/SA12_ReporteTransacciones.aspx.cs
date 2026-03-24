using System;
using System.Collections.Generic;
using System.Configuration;
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
        // ✅ SINGLETON
        private static readonly HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }

        // ✅ RegisterAsyncTask — el patrón correcto para async en WebForms
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(ConsultarReporteAsync));
        }

        private async Task ConsultarReporteAsync()
        {
            pnlMensaje.Visible    = false;
            pnlResultados.Visible = false;

            if (!DateTime.TryParse(txtFecha.Text.Trim(), out DateTime fechaSeleccionada))
            {
                MostrarMensaje("Por favor seleccione una fecha válida para generar el reporte.", false);
                return;
            }

            // Capturar token ANTES del await
            string token   = SessionHelper.AccessToken;
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/reports/transactions/daily?fecha={1}",
                            baseUrl.TrimEnd('/'),
                            fechaSeleccionada.ToString("yyyy-MM-dd"));

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var    response = await _httpClient.GetAsync(url);
                string body     = await response.Content.ReadAsStringAsync();

                dynamic resp        = JsonConvert.DeserializeObject(body);
                int     codigo      = (int)resp["codigo"];
                string  descripcion = (string)resp["descripcion"];

                if (codigo != 0)
                {
                    MostrarMensaje(descripcion, false);
                    return;
                }

                var data = resp["data"];
                var transacciones = JsonConvert.DeserializeObject<List<TransaccionReporteVM>>(
                    data["transacciones"].ToString());
                decimal totalDia = (decimal)data["totalMonto"];

                gvTransacciones.DataSource = transacciones;
                gvTransacciones.DataBind();

                lblFechaConsultada.Text = fechaSeleccionada.ToString("dd/MM/yyyy");
                lblTotalDia.Text        = totalDia.ToString("N2");
                pnlResultados.Visible   = true;
            }
            catch (Exception )
            {
                MostrarMensaje("No fue posible generar el reporte en este momento. Intente más tarde.", false);
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

    public class TransaccionReporteVM
    {
        public DateTime fecha           { get; set; }
        public string   telefonoOrigen  { get; set; }
        public string   telefonoDestino { get; set; }
        public decimal  monto           { get; set; }
    }
}
