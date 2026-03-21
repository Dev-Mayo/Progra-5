using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Admin
{
    public partial class SA12_ReporteTransacciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin.master valida sesión
            if (!IsPostBack)
            {
                // Precargar con la fecha de hoy
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible   = false;
            pnlResultados.Visible = false;

            if (!DateTime.TryParse(txtFecha.Text.Trim(), out DateTime fechaSeleccionada))
            {
                MostrarMensaje("La fecha indicada no es válida.", false);
                return;
            }

            // SRV17 ajustado para SA12: fecha OBLIGATORIA
            // GET /api/reports/transactions/daily?fecha=YYYY-MM-DD
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url     = string.Format("{0}/reports/transactions/daily?fecha={1}",
                                baseUrl.TrimEnd('/'),
                                fechaSeleccionada.ToString("yyyy-MM-dd"));

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + SessionHelper.AccessToken;

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader   = new StreamReader(response.GetResponseStream()))
                {
                    string respJson = reader.ReadToEnd();
                    dynamic resp    = JsonConvert.DeserializeObject(respJson);

                    int    codigo      = (int)(resp.codigo ?? resp.Codigo);
                    string descripcion = (string)(resp.descripcion ?? resp.Descripcion);

                    if (codigo != 0)
                    {
                        MostrarMensaje(descripcion, false);
                        return;
                    }

                    // DailyReportResponse: fecha, transacciones, totalMonto
                    var transacciones = JsonConvert.DeserializeObject<List<TransaccionReporteVM>>(
                        resp.data.transacciones.ToString()
                    );
                    decimal totalDia = (decimal)resp.data.totalMonto;

                    // SA12: Fecha, Teléfono origen, Teléfono destino, Monto
                    gvTransacciones.DataSource = transacciones;
                    gvTransacciones.DataBind();

                    // SA12: sumatoria del total de transacciones del día
                    lblFechaConsultada.Text = fechaSeleccionada.ToString("dd/MM/yyyy");
                    lblTotalDia.Text        = totalDia.ToString("N2");
                    pnlResultados.Visible   = true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        try
                        {
                            dynamic err  = JsonConvert.DeserializeObject(reader.ReadToEnd());
                            string  desc = (string)(err.descripcion ?? err.Descripcion ?? err.message);
                            MostrarMensaje(desc ?? "Error al obtener el reporte.", false);
                        }
                        catch { MostrarMensaje("Error al obtener el reporte.", false); }
                    }
                    return;
                }
                MostrarMensaje("No se pudo conectar con el servicio. Verifique que la API esté corriendo.", false);
            }
            catch
            {
                MostrarMensaje("Ocurrió un error inesperado.", false);
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
