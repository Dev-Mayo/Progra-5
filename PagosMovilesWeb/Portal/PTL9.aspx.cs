using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL9_Transfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionHelper.HaySesion())
            {
                Response.Redirect("~/FakeLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                txtNombreOrigen.Text = SessionHelper.NombreCompleto;
                pnlMensaje.Visible = false;
            }
        }

        protected void btnTransferir_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible = false;
            lblMensaje.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(SessionHelper.AccessToken))
            {
                MostrarMensaje("No autorizado.", false);
                return;
            }

            string baseUrl = ConfigurationManager.AppSettings["GatewayBaseUrl"];
            string url = baseUrl.TrimEnd('/') + "/gateway/trans/route";

            string json = ConstruirJsonSolicitud();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers["Authorization"] = "Bearer " + SessionHelper.AccessToken;

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string respuestaJson = reader.ReadToEnd();
                    string mensaje = ObtenerMensajeDesdeRespuesta(response.StatusCode, respuestaJson);

                    MostrarMensaje(mensaje, response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created);

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    {
                        LimpiarFormulario();
                        txtNombreOrigen.Text = SessionHelper.NombreCompleto;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var httpResponse = (HttpWebResponse)ex.Response;

                    using (var reader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string errorJson = reader.ReadToEnd();
                        string mensaje = ObtenerMensajeDesdeRespuesta(httpResponse.StatusCode, errorJson);
                        MostrarMensaje(mensaje, false);
                        return;
                    }
                }

                MostrarMensaje("No se pudo conectar con el servicio de transferencias.", false);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error inesperado: " + ex.Message, false);
            }
        }

        private string ConstruirJsonSolicitud()
        {
            var payload = new JObject();

            payload["TelefonoOrigen"] = txtTelefonoOrigen.Text.Trim();
            payload["NombreOrigen"] = txtNombreOrigen.Text.Trim();
            payload["TelefonoDestino"] = txtTelefonoDestino.Text.Trim();
            payload["Descripcion"] = txtDescripcion.Text.Trim();
            payload["EntidadDestino"] = string.IsNullOrWhiteSpace(txtEntidadDestino.Text)
                ? null
                : JToken.FromObject(txtEntidadDestino.Text.Trim());

            string montoTexto = txtMonto.Text.Trim();
            decimal monto;

            if (decimal.TryParse(montoTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out monto) ||
                decimal.TryParse(montoTexto, NumberStyles.Any, new CultureInfo("es-CR"), out monto))
            {
                payload["Monto"] = JToken.FromObject(monto);
            }
            else if (string.IsNullOrWhiteSpace(montoTexto))
            {
                payload["Monto"] = null;
            }
            else
            {
                payload["Monto"] = JToken.FromObject(montoTexto);
            }

            return payload.ToString(Formatting.None);
        }

        private string ObtenerMensajeDesdeRespuesta(HttpStatusCode statusCode, string cuerpo)
        {
            string mensajeJson = ExtraerMensajeJson(cuerpo);

            if (!string.IsNullOrWhiteSpace(mensajeJson))
                return mensajeJson;

            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    return "Debe revisar los datos enviados.";
                case HttpStatusCode.Unauthorized:
                    return "No autorizado.";
                case HttpStatusCode.NotFound:
                    return "Recurso no encontrado.";
                case HttpStatusCode.InternalServerError:
                    return "Ocurrió un error interno al procesar la transferencia.";
                case HttpStatusCode.GatewayTimeout:
                    return "El servicio tardó demasiado en responder.";
                default:
                    return "No fue posible procesar la transferencia.";
            }
        }

        private string ExtraerMensajeJson(string cuerpo)
        {
            if (string.IsNullOrWhiteSpace(cuerpo))
                return string.Empty;

            try
            {
                var token = JToken.Parse(cuerpo);

                if (token.Type != JTokenType.Object)
                    return cuerpo;

                var obj = (JObject)token;

                string descripcion = ObtenerValorTexto(obj, "descripcion", "Descripcion");
                if (!string.IsNullOrWhiteSpace(descripcion))
                    return descripcion;

                string detail = ObtenerValorTexto(obj, "detail", "Detail");
                if (!string.IsNullOrWhiteSpace(detail))
                    return detail;

                string title = ObtenerValorTexto(obj, "title", "Title");
                if (!string.IsNullOrWhiteSpace(title))
                    return title;

                var errors = obj["errors"] ?? obj["Errors"];
                if (errors != null && errors.Type == JTokenType.Object)
                {
                    var erroresObj = (JObject)errors;
                    foreach (var propiedad in erroresObj.Properties())
                    {
                        if (propiedad.Value != null && propiedad.Value.Type == JTokenType.Array)
                        {
                            foreach (var item in propiedad.Value)
                            {
                                string mensaje = item.ToString();
                                if (!string.IsNullOrWhiteSpace(mensaje))
                                    return mensaje;
                            }
                        }
                    }
                }

                return cuerpo;
            }
            catch
            {
                return cuerpo;
            }
        }

        private string ObtenerValorTexto(JObject obj, params string[] propiedades)
        {
            foreach (string propiedad in propiedades)
            {
                var valor = obj[propiedad];
                if (valor != null)
                {
                    string texto = valor.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(texto))
                        return texto;
                }
            }

            return string.Empty;
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = esExito
                ? "alert alert-success shadow-sm mb-4"
                : "alert alert-danger shadow-sm mb-4";

            lblMensaje.Text = Server.HtmlEncode(mensaje);
        }

        private void LimpiarFormulario()
        {
            txtTelefonoOrigen.Text = string.Empty;
            txtTelefonoDestino.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtEntidadDestino.Text = string.Empty;
        }
    }
}
