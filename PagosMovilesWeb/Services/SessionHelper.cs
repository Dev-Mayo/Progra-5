using System;
using System.Web;

namespace PagosMovilesWeb.Services
{
    public static class SessionHelper
    {
        public static string UsuarioId
        {
            get { return HttpContext.Current.Session["UsuarioId"]?.ToString() ?? ""; }
        }
        public static string NombreCompleto
        {
            get { return HttpContext.Current.Session["NombreCompleto"]?.ToString() ?? ""; }
        }
        public static string Rol
        {
            get { return HttpContext.Current.Session["Rol"]?.ToString() ?? ""; }
        }
        public static string AccessToken
        {
            get { return HttpContext.Current.Session["AccessToken"]?.ToString() ?? ""; }
        }

        public static bool HaySesion()
        {
            return HttpContext.Current.Session["UsuarioId"] != null
                && HttpContext.Current.Session["AccessToken"] != null;
        }

        public static void CerrarSesion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        // ── NUEVO: inactividad SA4/PTL4 ──────────────────────────────
        private const int TIMEOUT_MINUTOS = 5;

        public static bool SesionExpirada()
        {
            if (!HaySesion()) return true;
            var ultima = HttpContext.Current.Session["UltimaActividad"];
            if (ultima == null) return true;
            return (DateTime.Now - (DateTime)ultima).TotalMinutes >= TIMEOUT_MINUTOS;
        }

        public static void RenovarActividad()
        {
            if (HaySesion())
                HttpContext.Current.Session["UltimaActividad"] = DateTime.Now;
        }
    }
}