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

        private async Task CargarClientes()
        {
            var clientes = await GetAsync<List<Cliente>>($"{baseUrl}/core/client");

            if (clientes != null)
            {
                gvClientes.DataSource = clientes;
                gvClientes.DataBind();
            }
        }

        protected async void btnBuscar_Click(object sender, EventArgs e)
        {
            string identificacion = txtBuscarIdentificacion.Text.Trim();

            if (string.IsNullOrEmpty(identificacion))
                return;

            var clientes = await GetAsync<List<Cliente>>
                ($"{baseUrl}/core/client/Cliente?identificacion={identificacion}");

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
        }

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text))
                return;

            int telefono = 0;
            int.TryParse(txtTelefono.Text, out telefono);

            int tipoIdentificacion = 0;
            int.TryParse(ddlTipoIdentificacion.SelectedValue, out tipoIdentificacion);

            var cliente = new
            {
                identificacion = txtIdentificacion.Text,
                nombre = txtNombre.Text,
                apellido = txtApellido.Text,
                fecha_nacimiento = txtFechaNacimiento.Text,
                tipoIdentificacion = tipoIdentificacion,
                telefono = telefono,
                email = txtEmail.Text,
                contrasena = txtContrasena.Text
            };

            bool creado = await PostAsync($"{baseUrl}/core/client", cliente);

            if (creado)
            {
                await CargarClientes();
                LimpiarFormulario();
            }
        }

        protected async void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text))
                return;

            int? telefono = null;
            int parsedTelefono;

            if (int.TryParse(txtTelefono.Text, out parsedTelefono))
                telefono = parsedTelefono;

            int? tipoIdentificacion = null;
            int parsedTipo;

            if (int.TryParse(ddlTipoIdentificacion.SelectedValue, out parsedTipo))
                tipoIdentificacion = parsedTipo;

            var cliente = new
            {
                identificacion = txtIdentificacion.Text,
                nombre = string.IsNullOrWhiteSpace(txtNombre.Text) ? null : txtNombre.Text,
                apellido = string.IsNullOrWhiteSpace(txtApellido.Text) ? null : txtApellido.Text,
                fecha_nacimiento = string.IsNullOrWhiteSpace(txtFechaNacimiento.Text) ? null : txtFechaNacimiento.Text,
                tipoIdentificacion = tipoIdentificacion,
                telefono = telefono,
                email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text,
                contrasena = string.IsNullOrWhiteSpace(txtContrasena.Text) ? null : txtContrasena.Text
            };

            bool actualizado = await PutAsync($"{baseUrl}/core/client", cliente);

            if (actualizado)
            {
                await CargarClientes();
                LimpiarFormulario();
            }
        }

        protected async void btnEliminarCliente_Click(object sender, EventArgs e)
        {
            string identificacion = txtEliminarIdentificacion.Text.Trim();

            if (string.IsNullOrEmpty(identificacion))
                return;

            bool eliminado = await DeleteAsync($"{baseUrl}/core/client/{identificacion}");

            if (eliminado)
            {
                await CargarClientes();
                txtEliminarIdentificacion.Text = "";
            }
        }

        private void LimpiarFormulario()
        {
            txtIdentificacion.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtFechaNacimiento.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtContrasena.Text = "";
            ddlTipoIdentificacion.SelectedIndex = 0;
        }
    }
}