using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.DataAccess.Models;
using System.Globalization;

namespace ProyectoWebAPI.DataAccess
{
    public class ClienteRepository
    {
        private readonly ClienteContext _context;

        public ClienteRepository(ClienteContext context)
        {
            _context = context;
        }

        public List<Cliente> ObtenerTodos()
        {
            return _context.Clientes
                .FromSqlRaw("EXEC SP_Cliente_ObtenerTodos")
                .AsEnumerable()
                .ToList();
        }

        public List<Cliente> Filtrar(string identificacion, string nombre, string tipo)
        {
            return _context.Clientes
                .FromSqlRaw(
                    "EXEC SP_Cliente_Filtrar @identificacion={0}, @nombre={1}, @tipo={2}",
                    identificacion, nombre, tipo
                )
                .ToList();
        }

        public List<Cliente> ObtenerPorId(int clienteId)
        {
            return _context.Clientes
                .FromSqlRaw(
                    "EXEC SP_Cliente_ObtenerPorId @cliente_id ={0}",
                    clienteId
                )
                .ToList();
        }

        public void Insertar(Cliente c)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Cliente_Insertar @identificacion={0},@nombre={1},@apellido={2},@Email={3},@Tipo_Identificacion={4},@Telefono={5},@Rol={6},@ContrasenaHash={7}",
                c.identificacion, c.nombre, c.apellido, c.Email,
                c.Tipo_Identificacion, c.Telefono, c.Rol, c.ContrasenaHash
            );
        }

        public void Modificar(Cliente c)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Cliente_Modificar @cliente_id={0},@identificacion={1},@nombre={2},@apellido={3},@Email={4},@Tipo_Identificacion={5},@Telefono={6},@Rol={7},@ContrasenaHash={8}",
                c.cliente_id, c.identificacion, c.nombre, c.apellido, c.Email,
                c.Tipo_Identificacion, c.Telefono, c.Rol, c.ContrasenaHash
            );
        }

        public void Eliminar(int id)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Cliente_Eliminar @cliente_id={0}", id
            );
        }
    }
}
