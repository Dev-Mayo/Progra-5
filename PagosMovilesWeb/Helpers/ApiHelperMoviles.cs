using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace PagosMovilesWeb.Helpers
{
    /// <summary>
    /// Helper para consumir la API de Pagos Móviles (SRV11, SRV13, SRV17)
    /// Puerto: http://localhost:5248  (API de Paola, Avance 1)
    /// Mismo patrón que ApiHelperAdmin pero apunta al microservicio de pagos
    /// </summary>
    public static class ApiHelperMoviles
    {
        public static HttpClient GetClient()
        {
            var client = new HttpClient();

            // Leer el token de la sesión actual (guardado al hacer login)
            var token = HttpContext.Current.Session["AccessToken"]?.ToString();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            client.BaseAddress = new Uri("http://localhost:5248/");

            return client;
        }
    }
}
