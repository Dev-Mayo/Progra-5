using Newtonsoft.Json;
using PagosMovilesWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        private async Task<T> GetAsync<T>(string url)
        {
            using (HttpClient client = GetClient())
            {
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return default;

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

        private async Task<bool> DeleteAsync(string url)
        {
            using (HttpClient client = GetClient())
            {
                var response = await client.DeleteAsync(url);

                return response.IsSuccessStatusCode;
            }
        }

        private async Task CargarCuentas()
        {
            var cuentas = await GetAsync<List<Cuenta>>($"{baseUrl}/core/accounts");

            if (cuentas != null)
            {
                gvCuentas.DataSource = cuentas;
                gvCuentas.DataBind();
            }
        }

        protected async void btnBuscarCuenta_Click(object sender, EventArgs e)
        {
            string numeroCuenta = txtBuscarCuenta.Text.Trim();

            var cuenta = await GetAsync<Cuenta>($"{baseUrl}/core/accounts/{numeroCuenta}");

            if (cuenta != null)
            {
                gvCuentas.DataSource = new List<Cuenta> { cuenta };
                gvCuentas.DataBind();
            }
        }

        protected async void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string cliente = txtBuscarCliente.Text.Trim();

            var cuentas = await GetAsync<List<Cuenta>>($"{baseUrl}/core/accounts/client/{cliente}");

            if (cuentas != null)
            {
                gvCuentas.DataSource = cuentas;
                gvCuentas.DataBind();
            }
        }

        protected async void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            Cuenta cuenta = new Cuenta
            {
                numeroCuenta = txtNumeroCuenta.Text.Trim(),
                identificacionCliente = txtCliente.Text.Trim(),
                tipoCuenta = ddlTipoCuenta.SelectedValue,
                saldo = decimal.Parse(txtSaldo.Text)
            };

            bool creada = await PostAsync($"{baseUrl}/core/accounts", cuenta);

            if (creada)
            {
                await CargarCuentas();
                LimpiarFormulario();
            }
        }

        protected async void btnActualizarCuenta_Click(object sender, EventArgs e)
        {
            Cuenta cuenta = new Cuenta
            {
                numeroCuenta = txtNumeroCuenta.Text.Trim(),
                identificacionCliente = txtCliente.Text.Trim(),
                tipoCuenta = ddlTipoCuenta.SelectedValue,
                saldo = decimal.Parse(txtSaldo.Text)
            };

            bool actualizada = await PutAsync($"{baseUrl}/core/accounts", cuenta);

            if (actualizada)
            {
                await CargarCuentas();
                LimpiarFormulario();
            }
        }

        protected async void EliminarCuenta(object sender, EventArgs e)
        {
            var btn = (System.Web.UI.WebControls.Button)sender;
            string numeroCuenta = btn.CommandArgument;

            bool eliminada = await DeleteAsync($"{baseUrl}/core/accounts/{numeroCuenta}");

            if (eliminada)
            {
                await CargarCuentas();
            }
        }

        private void LimpiarFormulario()
        {
            txtNumeroCuenta.Text = "";
            txtCliente.Text = "";
            txtSaldo.Text = "";
            ddlTipoCuenta.SelectedIndex = 0;
        }
    }
}