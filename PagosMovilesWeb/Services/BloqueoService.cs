using System;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace PagosMovilesWeb.Services
{
    public static class BloqueoService
    {
        private static readonly string _connPagosMoviles =
            WebConfigurationManager.ConnectionStrings["PagosMoviles"].ConnectionString;
        private static readonly string _connCoreBancario =
            WebConfigurationManager.ConnectionStrings["CoreBancario"].ConnectionString;

        // ── Verificar si el usuario está bloqueado ───────────────────
        public static bool EstaBlockeado(string email)
        {
            bool esAdmin = UsuarioExisteEnBD(email, _connPagosMoviles, "Usuarios");
            if (esAdmin)
                return EsBloqueadoEnBD(email, _connPagosMoviles);

            return EsBloqueadoEnBD(email, _connCoreBancario);
        }

        private static bool EsBloqueadoEnBD(string email, string conn)
        {
            try
            {
                using (var cn = new SqlConnection(conn))
                {
                    cn.Open();
                    string query = @"SELECT Bloqueado FROM IntentosFallidos 
                                     WHERE Email = @email";
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return (bool)result;
                    }
                }
            }
            catch { }
            return false;
        }

        // ── Verificar en qué BD existe el usuario ────────────────────
        private static bool UsuarioExisteEnBD(string email, string conn, string tabla)
        {
            try
            {
                using (var cn = new SqlConnection(conn))
                {
                    cn.Open();
                    string query = $"SELECT COUNT(1) FROM {tabla} WHERE Email = @email";
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch { return false; }
        }

        // ── Registrar intento fallido ────────────────────────────────
        public static bool RegistrarIntentoFallido(string email)
        {
            // Determinar en cuál BD está el usuario
            bool esAdmin = UsuarioExisteEnBD(email, _connPagosMoviles, "Usuarios");

            if (esAdmin)
            {
                RegistrarEnBD(email, _connPagosMoviles);
                return EsBloqueadoEnBD(email, _connPagosMoviles);
            }
            else
            {
                RegistrarEnBD(email, _connCoreBancario);
                return EsBloqueadoEnBD(email, _connCoreBancario);
            }
        }

        private static void RegistrarEnBD(string email, string conn)
        {
            try
            {
                using (var cn = new SqlConnection(conn))
                {
                    cn.Open();
                    string query = @"
                        IF EXISTS (SELECT 1 FROM IntentosFallidos WHERE Email = @email)
                            UPDATE IntentosFallidos 
                            SET Intentos = Intentos + 1,
                                Bloqueado = CASE WHEN Intentos + 1 >= 3 THEN 1 ELSE 0 END
                            WHERE Email = @email
                        ELSE
                            INSERT INTO IntentosFallidos (Email, Intentos, Bloqueado)
                            VALUES (@email, 1, 0)";
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        // ── Resetear intentos al login exitoso ───────────────────────
        public static void ResetearIntentos(string email)
        {
            ResetearEnBD(email, _connPagosMoviles);
            ResetearEnBD(email, _connCoreBancario);
        }

        private static void ResetearEnBD(string email, string conn)
        {
            try
            {
                using (var cn = new SqlConnection(conn))
                {
                    cn.Open();
                    string query = @"UPDATE IntentosFallidos 
                                     SET Intentos = 0, Bloqueado = 0 
                                     WHERE Email = @email";
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }
    }
}