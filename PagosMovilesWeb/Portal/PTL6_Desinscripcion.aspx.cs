using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL6_Desinscripcion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE/USUARIO
        }

        protected void btnDesinscribir_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible = false;

            string telefono = txtTelefono.Text.Trim();
            string cuenta   = txtCuenta.Text.Trim();

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(telefono) || telefono.Length != 8)
            {
                MostrarMensaje("El número de teléfono debe tener 8 dígitos.", false);
                return;
            }
            if (string.IsNullOrWhiteSpace(cuenta))
            {
                MostrarMensaje("Debe ingresar el número de cuenta.", false);
                return;
            }

            // La identificación se obtiene del JWT (campo "id" del token)
            string identificacion = ObtenerIdentificacionDelToken();
            if (string.IsNullOrEmpty(identificacion))
            {
                MostrarMensaje("No se pudo obtener la identificación del usuario. Intente iniciar sesión nuevamente.", false);
                return;
            }

            // ⚠️ URL del servicio SRV10 del compañero (desinscripción)
            // Cambiar el puerto por el real del compañero de auth
            string url = "https://localhost:7160/auth/cancel-subscription";

            var body = new
            {
                numeroCuenta   = cuenta,
                identificacion = identificacion,
                numeroTelefono = telefono
            };

            string json    = JsonConvert.SerializeObject(body);
            var    request = (HttpWebRequest)WebRequest.Create(url);
            request.Method      = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Bearer " + SessionHelper.AccessToken;

            // Ignorar errores de certificado SSL en desarrollo
            request.ServerCertificateValidationCallback =
                (msg, cert, chain, errors) => true;

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(json);
                sw.Flush();
            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader   = new StreamReader(response.GetResponseStream()))
                {
                    string respJson = reader.ReadToEnd();
                    dynamic resp    = JsonConvert.DeserializeObject(respJson);

                    int    codigo      = (int)(resp.codigo ?? resp.Codigo);
                    string descripcion = (string)(resp.descripcion ?? resp.Descripcion);

                    if (codigo == 0)
                    {
                        // Mensaje exacto SRV10: "Desinscripción realizada"
                        MostrarMensaje(descripcion, true);
                        txtTelefono.Text = string.Empty;
                        txtCuenta.Text   = string.Empty;
                    }
                    else
                    {
                        // "Datos incorrectos" / "Teléfono no se encuentra afiliado"
                        MostrarMensaje(descripcion, false);
                    }
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
                            MostrarMensaje(desc ?? "Error al procesar la solicitud.", false);
                        }
                        catch { MostrarMensaje("Error al procesar la solicitud.", false); }
                    }
                    return;
                }
                MostrarMensaje("No se pudo conectar con el servicio. Intente más tarde.", false);
            }
            catch
            {
                MostrarMensaje("Ocurrió un error inesperado.", false);
            }
        }

        /// <summary>
        /// Decodifica el JWT para obtener el campo "id" = identificación del cliente.
        /// El JWT tiene 3 partes separadas por punto. La segunda parte es el payload en Base64.
        /// </summary>
        private string ObtenerIdentificacionDelToken()
        {
            try
            {
                string token  = SessionHelper.AccessToken;
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
            pnlMensaje.Visible    = true;
            pnlMensaje.CssClass   = esExito
                ? "alert alert-success shadow-sm mb-4"
                : "alert alert-danger shadow-sm mb-4";
            lblMensaje.Text = mensaje;
        }
    }
}
