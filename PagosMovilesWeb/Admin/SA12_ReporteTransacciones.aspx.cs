using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Admin
{
    public partial class SA12_ReporteTransacciones : System.Web.UI.Page
    {
        // ✅ SINGLETON correcto — un solo HttpClient para toda la aplicación
        private static readonly HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin.master valida sesión y rol ADMIN
            if (!IsPostBack)
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }

        // ✅ async void — funciona con Async="true" en el .aspx, sin deadlock
        protected async void btnConsultar_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible    = false;
            pnlResultados.Visible = false;

            if (!DateTime.TryParse(txtFecha.Text.Trim(), out DateTime fechaSeleccionada))
            {
                MostrarMensaje("Por favor seleccione una fecha válida para generar el reporte.", false);
                return;
            }

            // Capturar token antes del await
            string token   = SessionHelper.AccessToken;
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/reports/transactions/daily?fecha={1}",
                            baseUrl.TrimEnd('/'),
                            fechaSeleccionada.ToString("yyyy-MM-dd"));

            try
            {
                // ✅ Configurar singleton con el token capturado
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                // ✅ await real — sin deadlock gracias a Async="true" en el .aspx
                var response = await _httpClient.GetAsync(url);
                string body  = await response.Content.ReadAsStringAsync();

                dynamic resp        = JsonConvert.DeserializeObject(body);
                int     codigo      = (int)(resp.codigo ?? resp.Codigo);
                string  descripcion = (string)(resp.descripcion ?? resp.Descripcion);

                if (codigo != 0)
                {
                    MostrarMensaje(descripcion, false);
                    return;
                }

                var transacciones = JsonConvert.DeserializeObject<List<TransaccionReporteVM>>(
                    resp.data.transacciones.ToString()
                );
                decimal totalDia = (decimal)resp.data.totalMonto;

                gvTransacciones.DataSource = transacciones;
                gvTransacciones.DataBind();

                lblFechaConsultada.Text = fechaSeleccionada.ToString("dd/MM/yyyy");
                lblTotalDia.Text        = totalDia.ToString("N2");
                pnlResultados.Visible   = true;
            }
            catch (Exception)
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
