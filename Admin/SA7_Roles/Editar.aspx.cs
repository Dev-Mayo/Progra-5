using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PagosMovilesWeb.Models;

namespace PagosMovilesWeb.Admin.SA7_Roles
{
    public partial class Editar : System.Web.UI.Page
    {
        private const string ApiUrl = "https://localhost:5244/rol";
        private const string ApiUrlPantallas = "https://localhost:5244/screen";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPantallas();
                CargarRol();
            }
        }

        private void CargarPantallas()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.GetAsync(ApiUrlPantallas).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        var pantallas = JsonConvert.DeserializeObject<List<Pantalla>>(json);

                        cblPantallas.Items.Clear();
                        foreach (var p in pantallas)
                            cblPantallas.Items.Add(new ListItem(p.Nombre, p.Id.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar pantallas: {ex.Message}");
            }
        }

        private void CargarRol()
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
                        var rol = JsonConvert.DeserializeObject<Rol>(json);

                        txtId.Text = rol.Id.ToString();
                        txtNombre.Text = rol.Nombre;

                        // Marcar pantallas asociadas
                        if (rol.Pantallas != null)
                            foreach (ListItem item in cblPantallas.Items)
                                foreach (var p in rol.Pantallas)
                                    if (item.Value == p.Id.ToString())
                                        item.Selected = true;
                    }
                    else
                    {
                        MostrarMensaje("No se encontró el rol.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar rol: {ex.Message}");
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarMensaje("El nombre es obligatorio.");
                return;
            }

            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$"))
            {
                MostrarMensaje("El nombre solo puede contener letras, números y espacios.");
                return;
            }

            var pantallasSeleccionadas = new List<Pantalla>();
            foreach (ListItem item in cblPantallas.Items)
                if (item.Selected)
                    pantallasSeleccionadas.Add(new Pantalla { Id = int.Parse(item.Value), Nombre = item.Text });

            var rol = new Rol
            {
                Id = int.Parse(id),
                Nombre = nombre,
                Pantallas = pantallasSeleccionadas
            };

            try
            {
                string json = JsonConvert.SerializeObject(rol);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PutAsync($"{ApiUrl}/{id}", content).Result;

                    if (response.IsSuccessStatusCode)
                        Response.Redirect("~/Admin/SA7_Roles/Lista.aspx");
                    else
                        MostrarMensaje("Error al actualizar el rol.");
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