using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PagosMovilesWeb.Models;

namespace PagosMovilesWeb.Admin.SA6_Pantallas
{
    public partial class Crear : System.Web.UI.Page
    {
        private const string ApiUrl = "https://localhost:5244/screen";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();
            string ruta = txtRutaAcceso.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(ruta))
            {
                MostrarMensaje("Todos los campos son obligatorios.");
                return;
            }

            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$"))
            {
                MostrarMensaje("El nombre solo puede contener letras, números y espacios.");
                return;
            }

            if (!Regex.IsMatch(descripcion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$"))
            {
                MostrarMensaje("La descripción solo puede contener letras, números y espacios.");
                return;
            }

            var pantalla = new Pantalla
            {
                Nombre = nombre,
                Descripcion = descripcion,
                RutaAcceso = ruta
            };

            try
            {
                string json = JsonConvert.SerializeObject(pantalla);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsync(ApiUrl, content).Result;

                    if (response.IsSuccessStatusCode)
                        Response.Redirect("~/Admin/SA6_Pantallas/Lista.aspx");
                    else
                        MostrarMensaje("Error al guardar la pantalla.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}");
            }
        }

        private void MostrarMensaje(string texto)
        {
            lblMensaje.Text = texto;
            lblMensaje.ForeColor = System.Drawing.Color.Red;
            lblMensaje.BackColor = System.Drawing.Color.Transparent;
        }
    }
}