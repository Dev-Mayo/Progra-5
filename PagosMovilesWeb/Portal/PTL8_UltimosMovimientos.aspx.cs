using Newtonsoft.Json;
using PagosMovilesWeb.Helpers;
using PagosMovilesWeb.Services;
using System;
using System.Collections.Generic;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL8_UltimosMovimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE automáticamente
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            pnlError.Visible      = false;
            pnlResultados.Visible = false;

            string telefono = txtTelefono.Text.Trim();

            // La identificación sale de la sesión — el usuario ya hizo login
            string identificacion = SessionHelper.UsuarioId;

            try
            {
                // SRV11: GET http://localhost:5248/api/accounts/transactions?telefono=...&identificacion=...
                string url = string.Format(
                    "api/accounts/transactions?telefono={0}&identificacion={1}",
                    Uri.EscapeDataString(telefono),
                    Uri.EscapeDataString(identificacion)
                );

                using (var client = ApiHelperMoviles.GetClient())
                {
                    var response = client.GetAsync(url).Result;
                    string body  = response.Content.ReadAsStringAsync().Result;

                    dynamic resultado = JsonConvert.DeserializeObject(body);

                    int codigo = (int)resultado.codigo;

                    if (codigo != 0)
                    {
                        // Mensajes exactos SRV11:
                        // "Debe enviar los datos completos y válidos"
                        // "Cliente no asociado a pagos móviles"
                        lblError.Text    = (string)resultado.descripcion;
                        pnlError.Visible = true;
                        return;
                    }

                    // TransactionResponse: numeroCuenta, telefono, movimientos (lista de 5)
                    string numeroCuenta = (string)resultado.data.numeroCuenta;
                    string telRespuesta = (string)resultado.data.telefono;

                    var movimientos = JsonConvert.DeserializeObject<List<MovimientoVM>>(
                        resultado.data.movimientos.ToString()
                    );

                    lblNumeroCuenta.Text  = numeroCuenta;
                    lblTelefono.Text      = telRespuesta;

                    gvMovimientos.DataSource = movimientos;
                    gvMovimientos.DataBind();

                    pnlResultados.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text    = "Error al consultar los movimientos. Intente más tarde.";
                pnlError.Visible = true;
            }
        }

        /// <summary>
        /// Colorea el tipo de movimiento en la grilla:
        /// CREDITO = verde con flecha arriba
        /// DEBITO  = rojo con flecha abajo
        /// </summary>
        protected string FormatearTipo(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return tipo;

            if (tipo.ToUpper() == "CREDITO")
                return "<span style='color:green; font-weight:bold;'>▲ " + tipo + "</span>";
            else
                return "<span style='color:red; font-weight:bold;'>▼ " + tipo + "</span>";
        }
    }

    /// <summary>
    /// ViewModel que mapea exactamente MovimientoDto del API (JSON camelCase)
    /// </summary>
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
