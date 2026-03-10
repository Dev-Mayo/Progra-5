using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.DataAccess.Models;
using CoreBancarioService.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.DataAccess
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly CoreBancarioContext _context;

        public ClienteRepository(CoreBancarioContext context)
        {
            _context = context;
        }

        public void CrearCliente(
            string identificacion, string nombre, string apellido,
            DateOnly fecha_nacimiento, int TipoIdentificacion, int Telefono, string Email, string Contrasena)
        {
            var parametros = new[]
            {
                new SqlParameter("@identificacion", identificacion),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@apellido", apellido),
                new SqlParameter("@fecha_nacimiento",fecha_nacimiento),
                new SqlParameter("@TipoIdentificacion",TipoIdentificacion),
                new SqlParameter("@Telefono",Telefono),
                new SqlParameter("@Email",Email),
                new SqlParameter("@Contrasena", Contrasena)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_CrearCliente @identificacion, @nombre, @apellido, @fecha_nacimiento, @TipoIdentificacion, @Telefono, @Email, @Contrasena", // falta crear SP
                parametros
            );
        }

        public void EditarCliente(
            string identificacion, string? nombre, string? apellido,
            DateOnly? fecha_nacimiento, int? TipoIdentificacion, int? Telefono, string? Email, string? Contrasena)
        {
            var parametros = new[]
            {
                new SqlParameter("@identificacion", identificacion),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@apellido", apellido),
                new SqlParameter("@fecha_nacimiento",fecha_nacimiento),
                new SqlParameter("@TipoIdentificacion",TipoIdentificacion),
                new SqlParameter("@Telefono",Telefono),
                new SqlParameter("@Email",Email),
                new SqlParameter("@Contrasena", Contrasena)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_EditarCliente @identificacion, @nombre, @apellido, @fecha_nacimiento, @TipoIdentificacion, @Telefono", // falta crear SP
                parametros
            );
        }

        public void EliminarCliente(
            string identificacion)
        {
            var parametros = new[]
            {
                new SqlParameter("@identificacion", identificacion)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_EliminarCliente @identificacion",
                parametros
            );
        }

        public async Task<IEnumerable<ClienteResponse>> ListarTodos()
        {
            return await _context.Database
                .SqlQueryRaw<ClienteResponse>("EXEC sp_ListarTodos")
                .ToListAsync();
        }

        public async Task<IEnumerable<ClienteResponse>> ListarClientePorLlavePrimaria(string identificacion)
        {
            return await _context.Database
                .SqlQueryRaw<ClienteResponse>("EXEC sp_ListarClientePorLlavePrimaria @identificacion = @p0", identificacion)
                .ToListAsync();
        }
    }
}
