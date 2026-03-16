using Newtonsoft.Json;
using PagosMovilesWeb.Helpers;
using PagosMovilesWeb.Services;
using System;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL7_ConsultaSaldo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE automáticamente
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            pnlError.Visible    = false;
            pnlResultado.Visible = false;

            string telefono = txtTelefono.Text.Trim();

            // La identificación sale de la sesión — el usuario ya hizo login
            string identificacion = SessionHelper.UsuarioId;

            try
            {
                // SRV13: GET http://localhost:5248/api/accounts/balance?telefono=...&identificacion=...
                string url = string.Format(
                    "api/accounts/balance?telefono={0}&identificacion={1}",
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
                        // Mensajes exactos SRV13:
                        // "Debe enviar los datos completos y válidos"
                        // "Cliente no asociado a pagos móviles"
                        lblError.Text    = (string)resultado.descripcion;
                        pnlError.Visible = true;
                        return;
                    }

                    // BalanceResponse: numeroCuenta, saldo, telefono, identificacion
                    string  numeroCuenta = (string)resultado.data.numeroCuenta;
                    decimal saldo        = (decimal)resultado.data.saldo;
                    string  telRespuesta = (string)resultado.data.telefono;

                    lblNumeroCuenta.Text  = numeroCuenta;
                    lblTelefono.Text      = telRespuesta;
                    lblSaldo.Text         = saldo.ToString("N2");
                    pnlResultado.Visible  = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text    = "Error al consultar el saldo. Intente más tarde.";
                pnlError.Visible = true;
            }
        }
    }
}
