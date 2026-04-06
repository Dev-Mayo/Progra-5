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
    public partial class SA11_AdminCuentas : System.Web.UI.Page
    {
        // SINGLETON
        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly string baseUrl = "http://localhost:5227";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarCuentas();
            }
        }

        private async Task<(List<Cuenta> data, string error)> GetAsyncList(string url)
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

                System.Diagnostics.Debug.WriteLine($"GET {url}: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(json);
                    return (null, apiError?.detail ?? apiError?.title ?? "Revise los criterios de busqueda");
                }

                return (JsonConvert.DeserializeObject<List<Cuenta>>(json), null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        private async Task<(bool success, string error)> DeleteAsync(string url)
        {
            try
            {
                var token = Session["AccessToken"]?.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = null;

                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"DELETE {url}: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(content);
                    return (false, apiError?.detail ?? apiError?.title ?? "Error al borrar cliente, intente de nuevo");
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task CargarCuentas()
        {
            var result = await GetAsyncList($"{baseUrl}/core/accounts");

            if (result.error != null)
            {
                pnlMensaje.CssClass = "alert alert-danger";
                lblMensaje.Text = result.error;
                pnlMensaje.Visible = true;

                gvCuentas.DataSource = null;
                gvCuentas.DataBind();
                return;
            }

            gvCuentas.DataSource = result.data;
            gvCuentas.DataBind();
        }

        protected async void btnBuscarCuenta_Click(object sender, EventArgs e)
        {
            string numeroCuenta = txtBuscarCuenta.Text.Trim();

            var result = await GetAsyncList(
                $"{baseUrl}/core/accounts/Cuenta?NumeroCuenta={numeroCuenta}");

            if (result.error != null)
            {
                pnlMensaje.CssClass = "alert alert-danger";
                lblMensaje.Text = result.error;
                pnlMensaje.Visible = true;

                gvResultadoBusqueda.DataSource = null;
                gvResultadoBusqueda.DataBind();
                pnlResultadoBusqueda.Visible = true;
                return;
            }

            gvResultadoBusqueda.DataSource = result.data;
            gvResultadoBusqueda.DataBind();

            pnlResultadoBusqueda.Visible = true;
        }

        protected async void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string clienteId = txtBuscarCliente.Text.Trim();

            var result = await GetAsyncList(
                $"{baseUrl}/core/accounts/Cliente?ClienteID={clienteId}");

            if (result.error != null)
            {
                pnlMensaje.CssClass = "alert alert-danger";
                lblMensaje.Text = result.error;
                pnlMensaje.Visible = true;

                gvCuentasCliente.DataSource = null;
                gvCuentasCliente.DataBind();
                pnlCuentasCliente.Visible = true;
                return;
            }

            gvCuentasCliente.DataSource = result.data;
            gvCuentasCliente.DataBind();

            pnlCuentasCliente.Visible = true;
        }

        protected void lnkEditar_Command(object sender, CommandEventArgs e)
        {
            string[] partes = e.CommandArgument.ToString().Split('_');
            string clienteId = partes[0];
            string numeroCuenta = partes[1];
            Response.Redirect($"SA11_FormCuenta.aspx?edit={clienteId}_{numeroCuenta}");
        }

        protected async void lnkEliminarCuenta_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            string[] datos = btn.CommandArgument.Split('|');

            string clienteId = datos[0];
            string numeroCuenta = datos[1];

            string url = $"{baseUrl}/core/accounts/{clienteId}/{numeroCuenta}";

            var result = await DeleteAsync(url);

            if (result.success)
            {
                pnlMensaje.CssClass = "alert alert-success alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = $"Cuenta <strong>{numeroCuenta}</strong> eliminada correctamente.";
            }
            else
            {
                pnlMensaje.CssClass = "alert alert-danger alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = result.error;
            }

            pnlMensaje.Visible = true;

            await CargarCuentas();
        }

        protected void gvCuentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCuentas.PageIndex = e.NewPageIndex;
            RegisterAsyncTask(new PageAsyncTask(async () => await CargarCuentas()));
        }
    }
}