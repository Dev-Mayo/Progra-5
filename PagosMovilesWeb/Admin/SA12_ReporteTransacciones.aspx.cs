using Newtonsoft.Json;
using PagosMovilesWeb.Helpers;
using System;
using System.Collections.Generic;

namespace PagosMovilesWeb.Admin
{
    public partial class SA12_ReporteTransacciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin.master ya protege la sesión y el rol ADMIN
            if (!IsPostBack)
            {
                // Precargar con la fecha de hoy
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            pnlError.Visible      = false;
            pnlResultados.Visible = false;

            if (!DateTime.TryParse(txtFecha.Text.Trim(), out DateTime fechaSeleccionada))
            {
                MostrarError("La fecha indicada no tiene un formato válido.");
                return;
            }

            try
            {
                // SRV17 ajustado (Avance 2): fecha obligatoria
                // GET http://localhost:5248/api/reports/transactions/daily?fecha=YYYY-MM-DD
                string url = $"api/reports/transactions/daily?fecha={fechaSeleccionada:yyyy-MM-dd}";

                using (var client = ApiHelperMoviles.GetClient())
                {
                    var response = client.GetAsync(url).Result;
                    string body  = response.Content.ReadAsStringAsync().Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        dynamic error = JsonConvert.DeserializeObject(body);
                        MostrarError(error?.descripcion?.ToString() ?? "Error al obtener el reporte.");
                        return;
                    }

                    dynamic resultado = JsonConvert.DeserializeObject(body);

                    // DailyReportResponse: { fecha, transacciones:[...], totalMonto }
                    var transacciones = JsonConvert.DeserializeObject<List<TransaccionReporteVM>>(
                        resultado.data.transacciones.ToString()
                    );

                    decimal totalDia = (decimal)resultado.data.totalMonto;

                    // SA12: mostrar Fecha, Teléfono origen, Teléfono destino, Monto
                    gvTransacciones.DataSource = transacciones;
                    gvTransacciones.DataBind();

                    // SA12: sumatoria del total de transacciones realizadas en el día
                    lblFechaConsultada.Text = fechaSeleccionada.ToString("dd/MM/yyyy");
                    lblTotalDia.Text        = totalDia.ToString("N2");
                    pnlResultados.Visible   = true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al consultar el reporte: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text    = mensaje;
            pnlError.Visible = true;
        }
    }

    // Mapea exactamente TransaccionDetalle del API (JSON camelCase)
    public class TransaccionReporteVM
    {
        public DateTime fecha           { get; set; }
        public string   telefonoOrigen  { get; set; }
        public string   telefonoDestino { get; set; }
        public decimal  monto           { get; set; }
    }
}
