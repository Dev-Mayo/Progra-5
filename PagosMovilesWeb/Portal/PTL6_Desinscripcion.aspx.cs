using Newtonsoft.Json;
using PagosMovilesWeb.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PagosMovilesWeb.Portal
{
    public partial class PTL6_Desinscripcion : System.Web.UI.Page
    {
        // ⚠️ URL del servicio SRV10 del compañero que hizo la desinscripción
        // Endpoint: POST /auth/cancel-subscription
        // Ajustar el puerto según el equipo (preguntar al compañero)
        private const string URL_DESINSCRIPCION = "http://localhost:5041/auth/cancel-subscription";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Portal.master valida sesión y rol CLIENTE automáticamente
        }

        protected void btnDesinscribir_Click(object sender, EventArgs e)
        {
            pnlError.Visible = false;
            pnlExito.Visible = false;

            // La identificación sale de la sesión — el usuario ya hizo login
            string identificacion = SessionHelper.UsuarioId;
            string telefono       = txtTelefono.Text.Trim();
            string cuenta         = txtCuenta.Text.Trim();

            // SRV10 recibe: numeroCuenta, identificacion, numeroTelefono
            var datos = new
            {
                numeroCuenta   = cuenta,
                identificacion = identificacion,
                numeroTelefono = telefono
            };

            string json = JsonConvert.SerializeObject(datos);

            try
            {
                // Tomar el token de la sesión
                string token = SessionHelper.AccessToken;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    var response  = client.PostAsync(URL_DESINSCRIPCION, contenido).Result;
                    string body   = response.Content.ReadAsStringAsync().Result;

                    dynamic resultado = JsonConvert.DeserializeObject(body);

                    int codigo = (int)resultado.codigo;

                    if (codigo == 0)
                    {
                        // Mensaje exacto SRV10 (PDF pág.20): "Desinscripción realizada"
                        lblExito.Text    = (string)resultado.descripcion;
                        pnlExito.Visible = true;

                        // Limpiar campos después del éxito
                        txtTelefono.Text = string.Empty;
                        txtCuenta.Text   = string.Empty;
                    }
                    else
                    {
                        // Mensajes SRV10: "Datos incorrectos" / "Teléfono no se encuentra afiliado"
                        lblError.Text    = (string)resultado.descripcion;
                        pnlError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text    = "Error al procesar la solicitud. Intente más tarde.";
                pnlError.Visible = true;
            }
        }
    }
}
