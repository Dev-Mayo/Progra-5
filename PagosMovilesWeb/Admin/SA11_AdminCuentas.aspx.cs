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
        private readonly string baseUrl = "http://localhost:5227";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarCuentas();
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            var token = Session["AccessToken"]?.ToString();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        private async Task CargarCuentas()
        {
            using (var client = GetClient())
            {
                var response = await client.GetAsync($"{baseUrl}/core/accounts");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var cuentas = JsonConvert.DeserializeObject<List<Cuenta>>(json);
                    gvCuentas.DataSource = cuentas;
                    gvCuentas.DataBind();
                }
            }
        }

        protected async void btnBuscarCuenta_Click(object sender, EventArgs e)
        {
            string numeroCuenta = txtBuscarCuenta.Text;
            using (var client = GetClient())
            {
                var response = await client.GetAsync($"{baseUrl}/core/accounts/Cuenta?NumeroCuenta={numeroCuenta}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var cuenta = JsonConvert.DeserializeObject<List<Cuenta>>(json);
                    gvResultadoBusqueda.DataSource = cuenta;
                    gvResultadoBusqueda.DataBind();
                    pnlResultadoBusqueda.Visible = true;
                }
            }
        }

        protected async void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string clienteId = txtBuscarCliente.Text;
            using (var client = GetClient())
            {
                var response = await client.GetAsync($"{baseUrl}/core/accounts/Cliente?ClienteID={clienteId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var cuentas = JsonConvert.DeserializeObject<List<Cuenta>>(json);
                    gvCuentasCliente.DataSource = cuentas;
                    gvCuentasCliente.DataBind();
                    pnlCuentasCliente.Visible = true;
                }
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

            using (var client = GetClient())
            {
                var response = await client.DeleteAsync($"{baseUrl}/core/accounts/{clienteId}/{numeroCuenta}");
                System.Diagnostics.Debug.WriteLine($" Status: {response.StatusCode}");
            }

            pnlMensaje.CssClass = "alert alert-success alert-dismissible fade show shadow-sm mb-4";
            lblMensaje.Text = $"Cuenta <strong>{numeroCuenta}</strong> (Cliente: {clienteId}) eliminada correctamente.";
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