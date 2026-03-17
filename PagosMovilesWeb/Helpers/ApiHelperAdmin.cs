using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace PagosMovilesWeb.Helpers
{
    public static class ApiHelperAdmin
    {
        public static HttpClient GetClient()
        {
            var client = new HttpClient();

            var token = HttpContext.Current.Session["AccessToken"]?.ToString();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            client.BaseAddress = new Uri("http://localhost:5227/");

            return client;
        }
    }
}