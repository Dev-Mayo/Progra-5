using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PagosMovilesWeb.Models;

namespace PagosMovilesWeb.Admin.SA9_Entidades
{
    public partial class Lista : System.Web.UI.Page
    {
        private const string ApiUrl = "https://localhost:5244/entidad";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarEntidades();
        }

        private void CargarEntidades(string id = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    string url = string.IsNullOrEmpty(id) ? ApiUrl : $"{ApiUrl}/{id}";
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;

                        if (string.IsNullOrEmpty(id))
                        {
                            var lista = JsonConvert.DeserializeObject<List<Entidad>>(json);
                            gvEntidades.DataSource = lista;
                        }
                        else
                        {
                            var entidad = JsonConvert.DeserializeObject<Entidad>(json);
                            gvEntidades.DataSource = new List<Entidad> { entidad };
                        }
                        gvEntidades.DataBind();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró la entidad.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar entidades: {ex.Message}", false);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtBuscar.Text.Trim();
            CargarEntidades(string.IsNullOrEmpty(id) ? null : id);
        }

        protected void gvEntidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Admin/SA9_Entidades/Editar.aspx?id={id}");
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = client.DeleteAsync($"{ApiUrl}/{id}").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            MostrarMensaje("Entidad eliminada correctamente.", true);
                            CargarEntidades();
                        }
                        else
                        {
                            MostrarMensaje("Error al eliminar la entidad.", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje($"Error: {ex.Message}", false);
                }
            }
        }

        private void MostrarMensaje(string texto, bool exito)
        {
            lblMensaje.Text = texto;
            lblMensaje.ForeColor = exito
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
            lblMensaje.BackColor = System.Drawing.Color.Transparent;
        }
    }
}