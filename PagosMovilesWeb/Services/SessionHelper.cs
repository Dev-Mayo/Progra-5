using System.Web;

namespace PagosMovilesWeb.Services
{
    public static class SessionHelper
    {
        public static string UsuarioId
        {
            get { return HttpContext.Current.Session["UsuarioId"] != null ? HttpContext.Current.Session["UsuarioId"].ToString() : ""; }
        }

        public static string NombreCompleto
        {
            get { return HttpContext.Current.Session["NombreCompleto"] != null ? HttpContext.Current.Session["NombreCompleto"].ToString() : ""; }
        }

        public static string Rol
        {
            get { return HttpContext.Current.Session["Rol"] != null ? HttpContext.Current.Session["Rol"].ToString() : ""; }
        }

        public static string AccessToken
        {
            get { return HttpContext.Current.Session["AccessToken"] != null ? HttpContext.Current.Session["AccessToken"].ToString() : ""; }
        }

        public static bool HaySesion()
        {
            return HttpContext.Current.Session["UsuarioId"] != null
                && HttpContext.Current.Session["NombreCompleto"] != null
                && HttpContext.Current.Session["Rol"] != null;
        }

        public static void CerrarSesion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}