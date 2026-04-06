using Newtonsoft.Json;
using PagosMovilesWeb.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace PagosMovilesWeb.Admin
{
    public partial class SA11_FormCuenta : System.Web.UI.Page
    {
        // SINGLETON
        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly string baseUrl = "http://localhost:5227";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string editParam = Request.QueryString["edit"];
                if (!string.IsNullOrEmpty(editParam))
                {
                    await CargarCuentaParaEditar(editParam);
                }
                else
                {
                    lblTitulo.Text = "Nueva Cuenta";
                    lblSubtitulo.Text = "Complete los datos de la nueva cuenta";
                }
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
                    return (default, apiError?.detail ?? apiError?.title ?? "Error desconocido");
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
                    return (false, apiError?.detail ?? apiError?.title ?? "Error desconocido");
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
                    return (false, apiError?.detail ?? apiError?.title ?? "Error desconocido");
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task CargarCuentaParaEditar(string editParam)
        {
            try
            {
                string[] partes = editParam.Split('_');
                if (partes.Length != 2)
                {
                    MostrarModal(false, "Parámetro inválido");
                    return;
                }

                string numeroCuenta = partes[1];

                var result = await GetAsync<dynamic[]>(
                    $"{baseUrl}/core/accounts/Cuenta?NumeroCuenta={numeroCuenta}");

                if (result.error != null)
                {
                    MostrarModal(false, result.error);
                    return;
                }

                var cuentas = result.data;

                if (cuentas != null && cuentas.Length > 0)
                {
                    dynamic cuenta = cuentas[0];

                    txtClienteId.Text = cuenta.clienteId.ToString();
                    ddlTipoCuenta.SelectedValue = cuenta.tipoCuenta?.ToString() ?? "";

                    lblTitulo.Text = $"Editar Cuenta {numeroCuenta}";
                    lblSubtitulo.Text = $"Modifique los datos de la cuenta {numeroCuenta}";
                }
                else
                {
                    MostrarModal(false, "No se encontró la cuenta");
                }
            }
            catch (Exception ex)
            {
                MostrarModal(false, ex.Message);
            }
        }

        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string clienteIdStr = txtClienteId.Text.Trim();
                string tipoCuenta = ddlTipoCuenta.SelectedValue;

                if (string.IsNullOrEmpty(clienteIdStr) || string.IsNullOrEmpty(tipoCuenta))
                {
                    MostrarModal(false, "Cliente ID y Tipo de Cuenta son requeridos");
                    return;
                }

                int clienteId = int.Parse(clienteIdStr);
                string editParam = Request.QueryString["edit"];

                (bool success, string error) result;
                string mensaje = "";

                if (!string.IsNullOrEmpty(editParam))
                {
                    string[] partes = editParam.Split('_');
                    string numeroCuenta = partes[1];

                    var cuentaData = new
                    {
                        clienteId = clienteId,
                        numeroCuenta = numeroCuenta,
                        tipoCuenta = tipoCuenta
                    };

                    result = await PutAsync($"{baseUrl}/core/accounts", cuentaData);
                    mensaje = $"Cuenta <strong>{numeroCuenta}</strong> actualizada correctamente";
                }
                else
                {
                    var cuentaData = new
                    {
                        clienteId = clienteId,
                        tipoCuenta = tipoCuenta
                    };

                    result = await PostAsync($"{baseUrl}/core/accounts", cuentaData);
                    mensaje = "Cuenta creada correctamente";
                }

                if (result.success)
                {
                    MostrarModal(true, mensaje);

                    if (string.IsNullOrEmpty(editParam))
                        LimpiarFormulario();
                }
                else
                {
                    MostrarModal(false, result.error);
                }
            }
            catch (Exception ex)
            {
                MostrarModal(false, ex.Message);
            }
        }

        private void MostrarModal(bool success, string mensaje)
        {
            if (success)
            {
                modalHeader.Attributes["class"] = "modal-header bg-success text-white border-0";
                modalIconHeader.Attributes["class"] = "bi bi-check-circle-fill me-2";
                modalTitle.InnerText = "¡Operación Exitosa!";

                modalIconBody.Attributes["class"] = "bi bi-check-circle display-1 text-success mb-3 opacity-75";
                modalMessageClass.Attributes["class"] = "fw-bold text-success mb-2";

                btnModalAccion.Attributes["class"] = "btn btn-success btn-lg px-4 shadow-sm";
            }
            else
            {
                modalHeader.Attributes["class"] = "modal-header bg-danger text-white border-0";
                modalIconHeader.Attributes["class"] = "bi bi-exclamation-triangle-fill me-2";
                modalTitle.InnerText = "Error";

                modalIconBody.Attributes["class"] = "bi bi-exclamation-triangle display-1 text-danger mb-3 opacity-75";
                modalMessageClass.Attributes["class"] = "fw-bold text-danger mb-2";

                btnModalAccion.Attributes["class"] = "btn btn-danger btn-lg px-4 shadow-sm";
            }

            lblModalMensaje.Text = mensaje;

            ClientScript.RegisterStartupScript(this.GetType(), "showModal",
                @"setTimeout(function(){ 
            var modal = new bootstrap.Modal(document.getElementById('modalMensaje')); 
            modal.show(); 
        }, 300);", true);
        }

        private void LimpiarFormulario()
        {
            txtClienteId.Text = "";
            ddlTipoCuenta.SelectedIndex = 0;
        }
    }
}