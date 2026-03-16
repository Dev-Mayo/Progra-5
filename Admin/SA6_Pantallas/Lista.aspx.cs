using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PagosMovilesWeb.Models;

namespace PagosMovilesWeb.Admin.SA6_Pantallas
{
    public partial class Lista : System.Web.UI.Page
    {
        private const string ApiUrl = "https://localhost:5244/screen";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarPantallas();
        }

        private void CargarPantallas(string nombre = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    string url = string.IsNullOrEmpty(nombre) ? ApiUrl : $"{ApiUrl}?nombre={nombre}";
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        var lista = JsonConvert.DeserializeObject<List<Pantalla>>(json);
                        gvPantallas.DataSource = lista;
                        gvPantallas.DataBind();
                    }
                    else
                    {
                        MostrarMensaje("No se encontraron pantallas.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar pantallas: {ex.Message}", false);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscar.Text.Trim();
            CargarPantallas(string.IsNullOrEmpty(nombre) ? null : nombre);
        }

        protected void gvPantallas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Admin/SA6_Pantallas/Editar.aspx?id={id}");
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
                            MostrarMensaje("Pantalla eliminada correctamente.", true);
                            CargarPantallas();
                        }
                        else
                        {
                            MostrarMensaje("Error al eliminar la pantalla.", false);
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