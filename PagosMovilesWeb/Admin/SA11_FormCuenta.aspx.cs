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
                    // Modo CREACIÓN
                    lblTitulo.Text = "Nueva Cuenta";
                    lblSubtitulo.Text = "Complete los datos de la nueva cuenta";
                }
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

        private async Task CargarCuentaParaEditar(string editParam)
        {
            try
            {
                // DEBUG
                System.Diagnostics.Debug.WriteLine($" Edit param recibido: {editParam}");

                string[] partes = editParam.Split('_');
                if (partes.Length != 2)
                {
                    System.Diagnostics.Debug.WriteLine(" Edit param inválido");
                    return;
                }

                string clienteId = partes[0];
                string numeroCuenta = partes[1];

                System.Diagnostics.Debug.WriteLine($" Buscando: Cliente={clienteId}, Cuenta={numeroCuenta}");

                var cuentas = await GetAsync<dynamic[]>($"{baseUrl}/core/accounts/Cuenta?NumeroCuenta={numeroCuenta}");
                if (cuentas != null && cuentas.Length > 0)
                {
                    dynamic cuenta = cuentas[0];
                    txtClienteId.Text = cuenta.clienteId.ToString();
                    ddlTipoCuenta.SelectedValue = cuenta.tipoCuenta?.ToString() ?? "";

                    lblTitulo.Text = $"Editar Cuenta {numeroCuenta}";
                    lblSubtitulo.Text = $"Modifique los datos de la cuenta {numeroCuenta}";

                    System.Diagnostics.Debug.WriteLine(" Cuenta cargada correctamente");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("❌ No se encontró la cuenta");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" Error cargar cuenta: {ex.Message}");
                MostrarError($"Error cargando cuenta: {ex.Message}");
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
                    MostrarError("Cliente ID y Tipo de Cuenta son requeridos");
                    return;
                }

                int clienteId = int.Parse(clienteIdStr);
                string editParam = Request.QueryString["edit"];

                dynamic cuentaData;
                bool success;
                string operacion = "";

                if (!string.IsNullOrEmpty(editParam))
                {
                    // EDITAR
                    string[] partes = editParam.Split('_');
                    string numeroCuenta = partes[1];

                    cuentaData = new
                    {
                        clienteId = clienteId,
                        numeroCuenta = numeroCuenta,
                        tipoCuenta = tipoCuenta
                    };

                    success = await PutAsync($"{baseUrl}/core/accounts", cuentaData);
                    operacion = $"actualizada";
                    lblModalMensaje.Text = $"Cuenta <strong>{numeroCuenta}</strong> {operacion} correctamente";
                }
                else
                {
                    // CREAR
                    cuentaData = new
                    {
                        clienteId = clienteId,
                        tipoCuenta = tipoCuenta
                    };

                    success = await PostAsync($"{baseUrl}/core/accounts", cuentaData);
                    operacion = "creada";
                    lblModalMensaje.Text = $"Cuenta {operacion} correctamente";
                }

                System.Diagnostics.Debug.WriteLine($" {operacion.ToUpper()}: {JsonConvert.SerializeObject(cuentaData)}");

                if (success)
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "showModal",
                        @"setTimeout(function(){ 
                            var modal = new bootstrap.Modal(document.getElementById('modalSuccess')); 
                            modal.show(); 
                        }, 500);", true);

                    if (string.IsNullOrEmpty(editParam))
                        LimpiarFormulario();
                }
                else
                {
                    MostrarError($"Error al {operacion} la cuenta");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error btnGuardar: {ex.Message}");
                MostrarError($"Error: {ex.Message}");
            }
        }

        private void MostrarError(string mensaje)
        {
            pnlError.Visible = true;
            lblError.Text = mensaje;
        }

        private void LimpiarFormulario()
        {
            txtClienteId.Text = "";
            ddlTipoCuenta.SelectedIndex = 0;
        }
    }
}