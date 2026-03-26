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

        private async Task<List<Cuenta>> GetAsyncList(string url)
        {
            try
            {
                var token = Session["AccessToken"]?.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(url);
                System.Diagnostics.Debug.WriteLine($"GET {url}: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Cuenta>>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GET {url} error: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> DeleteAsync(string url)
        {
            try
            {
                var token = Session["AccessToken"]?.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync(url);
                System.Diagnostics.Debug.WriteLine($"DELETE {url}: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DELETE {url} error: {ex.Message}");
                return false;
            }
        }

        private async Task CargarCuentas()
        {
            var cuentas = await GetAsyncList($"{baseUrl}/core/accounts");

            if (cuentas != null)
            {
                gvCuentas.DataSource = cuentas;
                gvCuentas.DataBind();
            }
        }

        protected async void btnBuscarCuenta_Click(object sender, EventArgs e)
        {
            string numeroCuenta = txtBuscarCuenta.Text;
            var cuentas = await GetAsyncList($"{baseUrl}/core/accounts/Cuenta?NumeroCuenta={numeroCuenta}");

            if (cuentas != null)
            {
                gvResultadoBusqueda.DataSource = cuentas;
                gvResultadoBusqueda.DataBind();
                pnlResultadoBusqueda.Visible = true;
            }
        }

        protected async void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string clienteId = txtBuscarCliente.Text;
            var cuentas = await GetAsyncList($"{baseUrl}/core/accounts/Cliente?ClienteID={clienteId}");

            if (cuentas != null)
            {
                gvCuentasCliente.DataSource = cuentas;
                gvCuentasCliente.DataBind();
                pnlCuentasCliente.Visible = true;
            }
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

            System.Diagnostics.Debug.WriteLine($"ELIMINAR: Cliente={clienteId}, Cuenta={numeroCuenta}");

            string url = $"{baseUrl}/core/accounts/{clienteId}/{numeroCuenta}";
            bool eliminado = await DeleteAsync(url);

            if (eliminado)
            {
                pnlMensaje.CssClass = "alert alert-success alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = $"Cuenta <strong>{numeroCuenta}</strong> (Cliente: {clienteId}) eliminada correctamente.";
            }
            else
            {
                pnlMensaje.CssClass = "alert alert-danger alert-dismissible fade show shadow-sm mb-4";
                lblMensaje.Text = $"Error al eliminar cuenta <strong>{numeroCuenta}</strong> (Cliente: {clienteId})";
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