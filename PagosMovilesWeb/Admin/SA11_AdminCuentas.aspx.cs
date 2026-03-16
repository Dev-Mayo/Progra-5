using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PagosMovilesWeb.Models;

namespace PagosMovilesWeb.Admin
{
    public partial class SA11_AdminCuentas : System.Web.UI.Page
    {

        string baseUrl = "http://localhost:5227";

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

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

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

                    gvResultadoBusqueda.DataSource =  cuenta ;
                    gvResultadoBusqueda.DataBind();
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
                }
            }
        }

        protected async void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            var cuenta = new
            {
                clienteId = int.Parse(txtClienteId.Text),
                tipoCuenta = ddlTipoCuenta.SelectedValue
            };

            using (var client = GetClient())
            {
                var json = JsonConvert.SerializeObject(cuenta);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PostAsync($"{baseUrl}/core/accounts", content);
            }

            await CargarCuentas();
        }

        protected async void btnActualizarCuenta_Click(object sender, EventArgs e)
        {
            var cuenta = new
            {
                clienteId = int.Parse(txtClienteId.Text),
                numeroCuenta = txtNumeroCuenta.Text,
                tipoCuenta = ddlTipoCuenta.SelectedValue
            };

            using (var client = GetClient())
            {
                var json = JsonConvert.SerializeObject(cuenta);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PutAsync($"{baseUrl}/core/accounts", content);
            }

            await CargarCuentas();
        }

        protected async void btnEliminarCuenta_Click(object sender, EventArgs e)
        {
            string clienteId = txtEliminarCliente.Text;
            string numeroCuenta = txtEliminarCuenta.Text;

            using (var client = GetClient())
            {
                await client.DeleteAsync($"{baseUrl}/core/accounts/{clienteId}/{numeroCuenta}");
            }

            await CargarCuentas();
        }

    }
}