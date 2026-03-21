using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using Newtonsoft.Json;
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

            string errorValidacion = ValidarFormulario();
            if (!string.IsNullOrWhiteSpace(errorValidacion))
            {
                MostrarMensaje(errorValidacion, false);
                return;
            }

            if (string.IsNullOrWhiteSpace(SessionHelper.AccessToken))
            {
                MostrarMensaje("No hay token en sesión.", false);
                return;
            }

            decimal monto = decimal.Parse(txtMonto.Text.Trim(), CultureInfo.InvariantCulture);

            string baseUrl = ConfigurationManager.AppSettings["PagosMovilesApiBaseUrl"];
            string url = baseUrl.TrimEnd('/') + "/transactions/route";

            var body = new
            {
                TelefonoOrigen = txtTelefonoOrigen.Text.Trim(),
                NombreOrigen = txtNombreOrigen.Text.Trim(),
                TelefonoDestino = txtTelefonoDestino.Text.Trim(),
                Monto = monto,
                Descripcion = txtDescripcion.Text.Trim(),
                EntidadDestino = txtEntidadDestino.Text.Trim()
            };

            string json = JsonConvert.SerializeObject(body);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Bearer " + SessionHelper.AccessToken;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string respuestaJson = reader.ReadToEnd();

                    dynamic respuesta = JsonConvert.DeserializeObject(respuestaJson);

                    int codigo = respuesta.codigo != null ? (int)respuesta.codigo : (int)respuesta.Codigo;
                    string descripcion = respuesta.descripcion != null
                        ? (string)respuesta.descripcion
                        : (string)respuesta.Descripcion;

                    if (codigo == 0)
                    {
                        MostrarMensaje(descripcion, true);
                        LimpiarFormulario();
                        txtNombreOrigen.Text = SessionHelper.NombreCompleto;
                    }
                    else
                    {
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
                        string errorJson = reader.ReadToEnd();

                        try
                        {
                            dynamic error = JsonConvert.DeserializeObject(errorJson);

                            string descripcion = error.descripcion != null
                                ? (string)error.descripcion
                                : (string)error.Descripcion;

                            MostrarMensaje(descripcion, false);
                            return;
                        }
                        catch
                        {
                            MostrarMensaje("Error al procesar la transferencia.", false);
                            return;
                        }
                    }
                }

                MostrarMensaje("No se pudo conectar con el servicio de transferencias.", false);
            }
            catch
            {
                MostrarMensaje("Ocurrió un error inesperado.", false);
            }
        }

        private string ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtTelefonoOrigen.Text))
                return "Debe ingresar el teléfono origen.";

            if (!EsTelefonoValido(txtTelefonoOrigen.Text.Trim()))
                return "El teléfono origen debe tener 8 dígitos numéricos.";

            if (string.IsNullOrWhiteSpace(txtNombreOrigen.Text))
                return "Debe existir un nombre origen.";

            if (string.IsNullOrWhiteSpace(txtTelefonoDestino.Text))
                return "Debe ingresar el teléfono destino.";

            if (!EsTelefonoValido(txtTelefonoDestino.Text.Trim()))
                return "El teléfono destino debe tener 8 dígitos numéricos.";

            if (txtTelefonoOrigen.Text.Trim() == txtTelefonoDestino.Text.Trim())
                return "El teléfono origen y destino no pueden ser iguales.";

            if (string.IsNullOrWhiteSpace(txtMonto.Text))
                return "Debe ingresar el monto.";

            decimal monto;
            if (!decimal.TryParse(txtMonto.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out monto))
            {
                if (!decimal.TryParse(txtMonto.Text.Trim(), NumberStyles.Any, new CultureInfo("es-CR"), out monto))
                    return "El monto no es válido.";
            }

            if (monto <= 0)
                return "El monto debe ser mayor a 0.";

            if (monto > 100000)
                return "El monto no debe ser superior a 100.000.";

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                return "Debe ingresar la descripción.";

            if (txtDescripcion.Text.Trim().Length > 25)
                return "La descripción no puede superar 25 caracteres.";

            return string.Empty;
        }

        private bool EsTelefonoValido(string telefono)
        {
            if (telefono.Length != 8)
                return false;

            foreach (char c in telefono)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = esExito
                ? "alert alert-success shadow-sm mb-4"
                : "alert alert-danger shadow-sm mb-4";

            lblMensaje.Text = mensaje;
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