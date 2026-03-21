using Newtonsoft.Json;
using PagosMovilesWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace PagosMovilesWeb.Admin
{
    public partial class SA10_FormCliente : System.Web.UI.Page
    {
        private readonly string baseUrl = "http://localhost:5227";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string editId = Request.QueryString["edit"];
                if (!string.IsNullOrEmpty(editId))
                {
                    lblFormTitle.Text = "Editar Cliente";
                    lblBreadcrumb.Text = "Editar Cliente";
                    await CargarCliente(editId);
                }
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            var token = Session["AccessToken"]?.ToString();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        private async Task<T> GetAsync<T>(string url)
        {
            using (var client = GetClient())
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) return default;
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private async Task<bool> PostAsync<T>(string url, T data)
        {
            using (var client = GetClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
        }

        private async Task<bool> PutAsync<T>(string url, T data)
        {
            using (var client = GetClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                return response.IsSuccessStatusCode;
            }
        }

        private async Task CargarCliente(string identificacion)
        {
            var clientes = await GetAsync<List<Cliente>>($"{baseUrl}/core/client/Cliente?identificacion={identificacion}");
            if (clientes != null && clientes.Count > 0)
            {
                var cliente = clientes[0];
                txtIdentificacion.Text = cliente.identificacion;
                txtNombre.Text = cliente.nombre;
                txtApellido.Text = cliente.apellido;
                txtFechaNacimiento.Text = cliente.fecha_nacimiento;
                txtTelefono.Text = cliente.telefono?.ToString() ?? "";
                txtEmail.Text = cliente.email ?? "";
                ddlTipoIdentificacion.SelectedValue = cliente.tipoIdentificacion?.ToString() ?? "";
            }
        }

        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            var cliente = new
            {
                identificacion = txtIdentificacion.Text.Trim(),
                nombre = txtNombre.Text.Trim(),
                apellido = txtApellido.Text.Trim(),
                fecha_nacimiento = string.IsNullOrWhiteSpace(txtFechaNacimiento.Text) ? null : txtFechaNacimiento.Text,
                tipoIdentificacion = int.Parse(ddlTipoIdentificacion.SelectedValue),
                telefono = int.TryParse(txtTelefono.Text, out int tel) ? (int?)tel : null,
                email = txtEmail.Text.Trim(),
                contrasena = txtContrasena.Text
            };

            bool success;
            string editId = Request.QueryString["edit"];

            if (!string.IsNullOrEmpty(editId))
            {
                success = await PutAsync($"{baseUrl}/core/client", cliente);
                lblModalMensaje.Text = "Cliente actualizado correctamente";
            }
            else
            {
                success = await PostAsync($"{baseUrl}/core/client", cliente);
                lblModalMensaje.Text = "Cliente creado correctamente";
            }

            if (success)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showModal",
                    @"setTimeout(function(){ 
                        var modal = new bootstrap.Modal(document.getElementById('modalSuccess')); 
                        modal.show(); 
                    }, 500);", true);
            }
        }
    }
}