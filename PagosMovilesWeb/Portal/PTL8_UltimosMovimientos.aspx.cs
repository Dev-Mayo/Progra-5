using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL8_UltimosMovimientos : System.Web.UI.Page
    {
        // ✅ SINGLETON correcto — un solo HttpClient para toda la aplicación
        private static readonly HttpClient _httpClient = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE/USUARIO
        }

        // ✅ Convierte el cliente_id numérico a la cédula real
        private string ObtenerIdentificacionPorClienteId(string clienteId)
        {
            try
            {
                string connStr = WebConfigurationManager
                                    .ConnectionStrings["CoreBancario"].ConnectionString;
                using (var cn = new SqlConnection(connStr))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(
                        "SELECT identificacion FROM cliente WHERE cliente_id = @id", cn))
                    {
                        cmd.Parameters.AddWithValue("@id", clienteId);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? string.Empty;
                    }
                }
            }
            catch { return string.Empty; }
        }

        // ✅ async void — funciona con Async="true" en el .aspx, sin deadlock
        protected async void btnConsultar_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible    = false;
            pnlResultados.Visible = false;

            string telefono = txtTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(telefono) || telefono.Length != 8)
            {
                MostrarMensaje("El número de teléfono debe contener exactamente 8 dígitos.", false);
                return;
            }

            string identificacion = ObtenerIdentificacionPorClienteId(SessionHelper.UsuarioId);
            if (string.IsNullOrEmpty(identificacion))
            {
                MostrarMensaje("Su sesión no es válida. Por favor inicie sesión nuevamente.", false);
                return;
            }

            // Capturar token antes del await
            string token   = SessionHelper.AccessToken;
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = string.Format("{0}/api/accounts/transactions?telefono={1}&identificacion={2}",
                            baseUrl.TrimEnd('/'),
                            Uri.EscapeDataString(telefono),
                            Uri.EscapeDataString(identificacion));

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

                lblNumeroCuenta.Text = (string)resp.data.numeroCuenta;
                lblTelefono.Text     = (string)resp.data.telefono;

                var movimientos = JsonConvert.DeserializeObject<List<MovimientoVM>>(
                    resp.data.movimientos.ToString()
                );

                gvMovimientos.DataSource = movimientos;
                gvMovimientos.DataBind();
                pnlResultados.Visible = true;
            }
            catch (Exception)
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
            pnlMensaje.Visible  = true;
            pnlMensaje.CssClass = esExito
                ? "alert alert-success shadow-sm mb-4"
                : "alert alert-danger shadow-sm mb-4";
            lblMensaje.Text = mensaje;
        }
    }

    public class MovimientoVM
    {
        public int      movimientoId    { get; set; }
        public string   tipoMovimiento  { get; set; }
        public decimal  monto           { get; set; }
        public decimal  saldoAnterior   { get; set; }
        public decimal  saldoActual     { get; set; }
        public string   descripcion     { get; set; }
        public DateTime fechaMovimiento { get; set; }
    }
}
