using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.DataAccess.Models
{
    public class EntidadesBancariasRepository
    {
        private readonly EntidadesBancariasContext _context;
        public EntidadesBancariasRepository(EntidadesBancariasContext context)
        {
            _context = context;
        }
        public List<EntidadesBancarias> ObtenerTodos()
        {
            return _context.EntidadesBancarias
                .FromSqlRaw("EXEC SP_Entidad_ObtenerTodos")
                .AsEnumerable()
                .ToList();
        }
        public List<EntidadesBancarias> ObtenerPorId(int entidad_id)
        {
            return _context.EntidadesBancarias
                .FromSqlRaw(
                    "EXEC SP_Entidad_ObtenerPorId @ID_Entidad ={0}",
                    entidad_id
                )
                .ToList();
        }
        public void Crear(EntidadesBancarias e)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Entidad_Insertar @nombre={0},@Estado={1}",
                e.Nombre, e.Estado
            );
        }
        public void Modificar(EntidadesBancarias e)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Entidad_Modificar @ID_Entidad={0}, @Nombre={1},@Estado={2}",
                e.ID_Entidad,e.Nombre, e.Estado
            );
        }
        public void Eliminar(int ID_Entidad)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Entidad_Eliminar @ID_Entidad={0}",
                ID_Entidad
            );
        }
    }
}
