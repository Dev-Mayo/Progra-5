using Newtonsoft.Json;
using PagosMovilesWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagosMovilesWeb.Admin
{
    public partial class SA10_AdminClientes : System.Web.UI.Page
    {
        private readonly string baseUrl = "http://localhost:5227";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarClientes();
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();  //cambiar esto - usar patron singleton o similar para evitar problemas de socket
            var token = Session["AccessToken"]?.ToString();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //prueba
            }
            return client;
        }

        private async Task<T> GetAsync<T>(string url)
        {
            using (HttpClient client = GetClient())
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) return default;
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private async Task<bool> PostAsync<T>(string url, T data)
        {
            using (HttpClient client = GetClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
        }

        private async Task<bool> PutAsync<T>(string url, T data)
        {
            using (HttpClient client = GetClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                return response.IsSuccessStatusCode;
            }
        }

        private async Task<bool> DeleteAsync(string id)
        {
            try
            {
                using (HttpClient client = GetClient())
                {
                    string url = $"{baseUrl}/core/client/{id}";
                    System.Diagnostics.Debug.WriteLine($"DELETE: {url}");

                    HttpResponseMessage response = await client.DeleteAsync(url);
                    string content = await response.Content.ReadAsStringAsync();

                    System.Diagnostics.Debug.WriteLine($"Status: {response.StatusCode}");
                    System.Diagnostics.Debug.WriteLine($"Response: {content}");

                    return response.IsSuccessStatusCode; //hay que implementar mensajes de error en base al status code del api
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error DELETE: {ex.Message}");
                return false;
            }
        }

        private async Task CargarClientes()
        {
            var clientes = await GetAsync<List<Cliente>>($"{baseUrl}/core/client");

            if (clientes != null)
            {
                gvClientes.DataSource = clientes;
                gvClientes.DataBind();
                lblTotalClientes.Text = clientes.Count.ToString();
            }
            else
            {
                lblTotalClientes.Text = "0";
            }
        }

        protected async void btnBuscar_Click(object sender, EventArgs e)
        {
            string identificacion = txtBuscarIdentificacion.Text.Trim();

            if (string.IsNullOrEmpty(identificacion))
            {
                pnlResultados.Visible = false;
                return;
            }

            var clientes = await GetAsync<List<Cliente>>(
                $"{baseUrl}/core/client/Cliente?identificacion={identificacion}");

            if (clientes != null && clientes.Count > 0)
            {
                gvResultadoBusqueda.DataSource = clientes;
                gvResultadoBusqueda.DataBind();
                pnlResultados.Visible = true;
            }
            else
            {
                gvResultadoBusqueda.DataSource = null;
                gvResultadoBusqueda.DataBind();
                pnlResultados.Visible = true;
            }
        }

        protected async void gvClientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                string identificacion = e.CommandArgument.ToString();

                bool eliminado = await DeleteAsync(identificacion);  //

                if (eliminado)
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = $"Cliente {identificacion} eliminado correctamente.";
                    await CargarClientes();
                }
                else
                {
                    pnlMensaje.CssClass = "alert alert-danger shadow-sm mb-4";
                    lblMensaje.Text = $"Error al eliminar cliente {identificacion}";
                    pnlMensaje.Visible = true;
                }
            }
        }

        protected void gvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClientes.PageIndex = e.NewPageIndex;
            RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                await CargarClientes();
            }));
        }

        public string ClienteIdParaEliminar
        {
            get { return ViewState["ClienteIdParaEliminar"]?.ToString() ?? ""; }
            set { ViewState["ClienteIdParaEliminar"] = value; }
        }

        protected async void lnkEliminar_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            string identificacion = btn.CommandArgument ?? ""; 
            System.Diagnostics.Debug.WriteLine($"🔍 Click recibido - ID: '{identificacion}' (Length: {identificacion.Length})");

            if (string.IsNullOrEmpty(identificacion))
            {
                pnlMensaje.CssClass = "alert alert-danger alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = "Error: No se pudo obtener el ID del cliente";
                pnlMensaje.Visible = true;
                return;
            }

            bool eliminado = await DeleteAsync(identificacion);

            if (eliminado)
            {
                pnlMensaje.CssClass = "alert alert-success alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = $"Cliente <strong>{identificacion}</strong> eliminado correctamente.";
                pnlMensaje.Visible = true;
                await CargarClientes();
            }
            else
            {
                pnlMensaje.CssClass = "alert alert-danger alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = $"Error al eliminar cliente <strong>{identificacion}</strong>";
                pnlMensaje.Visible = true;
            }
        }
    }
}