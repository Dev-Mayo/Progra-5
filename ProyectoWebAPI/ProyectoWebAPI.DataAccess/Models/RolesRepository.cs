using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.DataAccess.Models
{
    public class RolesRepository
    {
        private readonly RolesContext _context;
        public RolesRepository(RolesContext context)
        {
            _context = context;
        }
        public List<Roles> ObtenerTodos()
        {
            return _context.Roles
                .FromSqlRaw("EXEC SP_Rol_ObtenerTodos")
                .AsEnumerable()
                .ToList();
        }

        public List<Roles> ObtenerPorId(int id)
        {
            return _context.Roles
                .FromSqlRaw("EXEC SP_Rol_ObtenerPorId @ID_Rol = {0}", id)
                .AsEnumerable()
                .ToList();
        }

        public void Agregar(Roles r)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Rol_Insertar @Nombre = {0}, @Estado= {1}",
                r.Nombre, r.Estado);
        }

        public void Modificar(Roles r)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Rol_Modificar @ID_Rol = {0},@Nombre = {1}, @Estado= {2}",
                r.ID_Rol,r.Nombre, r.Estado);
        }

        public void Eliminar(int id)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Rol_Eliminar @ID_Rol = {0}",
                id);
        }
    }
}
