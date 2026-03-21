using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL7_ConsultaSaldo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE/USUARIO
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible  = false;
            pnlResultado.Visible = false;

            string telefono = txtTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(telefono) || telefono.Length != 8)
            {
                MostrarMensaje("El número de teléfono debe tener 8 dígitos.", false);
                return;
            }

            // La identificación sale del JWT guardado en sesión
            string identificacion = SessionHelper.UsuarioId;
            if (string.IsNullOrEmpty(identificacion))
            {
                MostrarMensaje("No se pudo obtener la identificación. Intente iniciar sesión nuevamente.", false);
                return;
            }

            // SRV13: GET /api/accounts/balance?telefono=...&identificacion=...
            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url     = string.Format("{0}/api/accounts/balance?telefono={1}&identificacion={2}",
                                baseUrl.TrimEnd('/'),
                                Uri.EscapeDataString(telefono),
                                Uri.EscapeDataString(identificacion));

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
                        // "Debe enviar los datos completos y válidos"
                        // "Cliente no asociado a pagos móviles"
                        MostrarMensaje(descripcion, false);
                        return;
                    }

                    // BalanceResponse: numeroCuenta, saldo, telefono, identificacion
                    lblNumeroCuenta.Text = (string)resp.data.numeroCuenta;
                    lblSaldo.Text        = ((decimal)resp.data.saldo).ToString("N2");
                    lblTelefono.Text     = (string)resp.data.telefono;

                    pnlResultado.Visible = true;
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
                            MostrarMensaje(desc ?? "Error al consultar el saldo.", false);
                        }
                        catch { MostrarMensaje("Error al consultar el saldo.", false); }
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

        private string ObtenerIdentificacionDelToken()
        {
            try
            {
                string token    = SessionHelper.AccessToken;
                string[] partes = token.Split('.');
                if (partes.Length != 3) return string.Empty;

                string payload = partes[1];
                int mod = payload.Length % 4;
                if (mod == 2) payload += "==";
                else if (mod == 3) payload += "=";

                byte[]  bytes   = Convert.FromBase64String(payload);
                string  json    = Encoding.UTF8.GetString(bytes);
                dynamic decoded = JsonConvert.DeserializeObject(json);

                return (string)decoded.id ?? string.Empty;
            }
            catch { return string.Empty; }
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
}
