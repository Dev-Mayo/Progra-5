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

        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly string baseUrl = "http://localhost:5227";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarClientes();
            }
        }

        private async Task<(T data, string error)> GetAsync<T>(string url)
        {
            try
            {
                var token = Session["AccessToken"]?.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = null;

                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(json);
                    return (default, apiError?.detail ?? apiError?.title ?? "Revise los criterios de busqueda");
                }

                return (JsonConvert.DeserializeObject<T>(json), null);
            }
            catch (Exception ex)
            {
                return (default, ex.Message);
            }
        }

        private async Task<(bool success, string error)> PostAsync<T>(string url, T data)
        {
            try
            {
                var token = Session["AccessToken"]?.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = null;

                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(responseContent);
                    return (false, apiError?.detail ?? apiError?.title);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task<(bool success, string error)> PutAsync<T>(string url, T data)
        {
            try
            {
                var token = Session["AccessToken"]?.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(responseContent);
                    return (false, apiError?.detail ?? apiError?.title);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task<(bool success, string error)> DeleteAsync(string id)
        {
            try
            {
                var token = Session["AccessToken"]?.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = null;

                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                string url = $"{baseUrl}/core/client/{id}";
                var response = await _httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(content);
                    return (false, apiError?.detail ?? apiError?.title);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task CargarClientes()
        {
            var result = await GetAsync<List<Cliente>>($"{baseUrl}/core/client");

            if (result.error != null)
            {
                pnlMensaje.CssClass = "alert alert-danger";
                lblMensaje.Text = result.error;
                pnlMensaje.Visible = true;

                gvClientes.DataSource = null;
                gvClientes.DataBind();
                lblTotalClientes.Text = "0";
                return;
            }

            var clientes = result.data;

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

            var result = await GetAsync<List<Cliente>>(
                $"{baseUrl}/core/client/Cliente?identificacion={identificacion}");

            if (result.error != null)
            {
                pnlMensaje.CssClass = "alert alert-danger";
                lblMensaje.Text = result.error;
                pnlMensaje.Visible = true;

                gvResultadoBusqueda.DataSource = null;
                gvResultadoBusqueda.DataBind();
                pnlResultados.Visible = true;
                return;
            }

            var clientes = result.data;

            if (clientes != null && clientes.Count > 0)
            {
                gvResultadoBusqueda.DataSource = clientes;
                gvResultadoBusqueda.DataBind();
            }
            else
            {
                gvResultadoBusqueda.DataSource = null;
                gvResultadoBusqueda.DataBind();
            }

            pnlResultados.Visible = true;
        }

        protected async void gvClientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                string identificacion = e.CommandArgument.ToString();
                var result = await DeleteAsync(identificacion);

                if (result.success)
                {
                    pnlMensaje.CssClass = "alert alert-success";
                    lblMensaje.Text = $"Cliente {identificacion} eliminado correctamente.";
                }
                else
                {
                    pnlMensaje.CssClass = "alert alert-danger";
                    lblMensaje.Text = result.error;
                }

                pnlMensaje.Visible = true;
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
            System.Diagnostics.Debug.WriteLine($" Click recibido - ID: '{identificacion}' (Length: {identificacion.Length})");

            if (string.IsNullOrEmpty(identificacion))
            {
                pnlMensaje.CssClass = "alert alert-danger alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = "Error: No se pudo obtener el ID del cliente";
                pnlMensaje.Visible = true;
                return;
            }

            var result = await DeleteAsync(identificacion);

            if (result.success)
            {
                pnlMensaje.CssClass = "alert alert-success";
                lblMensaje.Text = $"Cliente {identificacion} eliminado correctamente.";
            }
            else
            {
                pnlMensaje.CssClass = "alert alert-danger";
                lblMensaje.Text = result.error;
            }

            pnlMensaje.Visible = true;
        }
    }
    public class ApiError
    {
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
    }
}