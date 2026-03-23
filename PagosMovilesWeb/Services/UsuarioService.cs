using System.Data.SqlClient;
using System.Web.Configuration;

namespace PagosMovilesWeb.Services
{
    public class DatosUsuario
    {
        public string Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
    }

    public static class UsuarioService
    {
        private static readonly string _connPagosMoviles =
            WebConfigurationManager.ConnectionStrings["PagosMoviles"].ConnectionString;
        private static readonly string _connCoreBancario =
            WebConfigurationManager.ConnectionStrings["CoreBancario"].ConnectionString;

        public static DatosUsuario ObtenerDatos(string email)
        {
            // Buscar en Pagos_Moviles primero (admins)
            var datos = BuscarEnBD(email, _connPagosMoviles, "Usuarios",
                "CAST(IdUsuario AS VARCHAR)", "Email", "ADMIN");  // ← AGREGAR ESTA LÍNEA
            if (datos != null) return datos;

            // Buscar en CoreBancario (clientes)
            return BuscarEnBD(email, _connCoreBancario, "cliente",
                "CAST(cliente_id AS VARCHAR)", "Email", "CLIENTE/USUARIO",
                "nombre", "apellido");
        }

        private static DatosUsuario BuscarEnBD(string email, string conn,
            string tabla, string idCol, string emailCol, string rol,
            string nombreCol = null, string apellidoCol = null)
        {
            try
            {
                using (var cn = new SqlConnection(conn))
                {
                    cn.Open();
                    string selectNombre = nombreCol != null
                        ? $"{nombreCol} + ' ' + {apellidoCol}"
                        : "Email";
                    string query = $@"SELECT {idCol}, {selectNombre} 
                                     FROM {tabla} 
                                     WHERE {emailCol} = @email";
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new DatosUsuario
                                {
                                    Id = reader[0].ToString(),
                                    NombreCompleto = reader[1].ToString(),
                                    Rol = rol
                                };
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }
    }
}