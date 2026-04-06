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

        private async Task<(T data, string error)> GetAsync<T>(string url)
        {
            try
            {
                using (var client = GetClient())
                {
                    var response = await client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var apiError = JsonConvert.DeserializeObject<ApiError>(json);
                        return (default, apiError?.detail ?? apiError?.title ?? "El cliente o cuenta buscado no existe o se encuentra inactivo");
                    }

                    return (JsonConvert.DeserializeObject<T>(json), null);
                }
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
                using (var client = GetClient())
                {
                    var json = JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var apiError = JsonConvert.DeserializeObject<ApiError>(responseContent);
                        return (false, apiError?.detail ?? apiError?.title ?? "Error al crear el cliente, verifique la informacion ingresada");
                    }

                    return (true, null);
                }
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
                using (var client = GetClient())
                {
                    var json = JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var apiError = JsonConvert.DeserializeObject<ApiError>(responseContent);
                        return (false, apiError?.detail ?? apiError?.title ?? "Error desconocido");
                    }

                    return (true, null);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task CargarCliente(string identificacion)
        {
            var result = await GetAsync<List<Cliente>>(
                $"{baseUrl}/core/client/Cliente?identificacion={identificacion}");

            if (result.error != null)
            {
                lblModalMensaje.Text = result.error;
                return;
            }

            var clientes = result.data;

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

            string editId = Request.QueryString["edit"];

            (bool success, string error) result;

            if (!string.IsNullOrEmpty(editId))
            {
                result = await PutAsync($"{baseUrl}/core/client", cliente);
            }
            else
            {
                result = await PostAsync($"{baseUrl}/core/client", cliente);
            }

            if (result.success)
            {
                modalHeader.Attributes["class"] = "modal-header bg-success text-white";
                modalTitle.InnerText = "Éxito";
                lblModalMensaje.Text = "Cliente guardado correctamente";
            }
            else
            {
                modalHeader.Attributes["class"] = "modal-header bg-danger text-white border-0";
                modalIconHeader.Attributes["class"] = "bi bi-exclamation-triangle-fill me-2";
                modalTitle.InnerText = "Error";

                modalIconBody.Attributes["class"] = "bi bi-exclamation-triangle display-1 text-danger mb-3 opacity-75";

                modalMessageClass.Attributes["class"] = "fw-bold mb-2 text-danger";

                lblModalMensaje.Text = result.error;
                modalSubText.InnerText = "Por favor intente nuevamente";

                btnModalAccion.Attributes["class"] = "btn btn-danger btn-lg px-4 shadow-sm";
            }

            ClientScript.RegisterStartupScript(this.GetType(), "showModal",
                @"setTimeout(function(){ 
                    var modal = new bootstrap.Modal(document.getElementById('modalMensaje')); 
                    modal.show(); 
                }, 300);", true);
        }
    }
}