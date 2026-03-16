using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PagosMovilesWeb.Models;

namespace PagosMovilesWeb.Admin.SA9_Entidades
{
    public partial class Editar : System.Web.UI.Page
    {
        private const string ApiUrl = "https://localhost:5244/entidad";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarEntidad();
        }

        private void CargarEntidad()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) return;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.GetAsync($"{ApiUrl}/{id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        var entidad = JsonConvert.DeserializeObject<Entidad>(json);

                        txtId.Text = entidad.Id.ToString();
                        txtNombre.Text = entidad.Nombre;
                        txtUrl.Text = entidad.UrlExterna;
                        ddlEstado.SelectedValue = entidad.Estado;
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la entidad.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar entidad: {ex.Message}");
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string url = txtUrl.Text.Trim();
            string estado = ddlEstado.SelectedValue;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(url))
            {
                MostrarMensaje("Todos los campos son obligatorios.");
                return;
            }

            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                MostrarMensaje("El nombre solo puede contener letras y espacios.");
                return;
            }

            var entidad = new Entidad
            {
                Id = int.Parse(id),
                Nombre = nombre,
                UrlExterna = url,
                Estado = estado
            };

            try
            {
                string json = JsonConvert.SerializeObject(entidad);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PutAsync($"{ApiUrl}/{id}", content).Result;

                    if (response.IsSuccessStatusCode)
                        Response.Redirect("~/Admin/SA9_Entidades/Lista.aspx");
                    else
                        MostrarMensaje("Error al actualizar la entidad.");
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