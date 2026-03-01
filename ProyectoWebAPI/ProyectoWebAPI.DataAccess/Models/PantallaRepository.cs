using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.DataAccess.Models
{
    public class PantallaRepository
    {
        private readonly PantallaContext _context;

        public PantallaRepository(PantallaContext context)
        {
            _context = context;
        }

        public List<Pantalla> ObtenerTodos()
        {
            return _context.Pantallas
                .FromSqlRaw("EXEC SP_Pantalla_ObtenerTodos")
                .ToList();
        }

        public List<Pantalla> ObtenerPorId(int id)
        {
            return _context.Pantallas
                .FromSqlRaw(
                    "EXEC SP_Pantalla_ObtenerPorId @ID_Pantalla ={0}",
                    id
                )
                .ToList();
        }

        public void Insertar(Pantalla p)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Pantalla_Insertar @Nombre_Pantalla = {0}, @Descripcion= {1}, @Ruta_Acceso= {2}, @Estado= {3}",
                p.Nombre_Pantalla, p.Descripcion, p.Ruta_Acceso, p.Estado
            );
        }
        public void Modificar(Pantalla p)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Pantalla_Modificar @ID_Pantalla = {0},@Nombre_Pantalla = {1}, @Descripcion= {2}, @Ruta_Acceso= {3}, @Estado= {4}",
                p.ID_Pantalla,p.Nombre_Pantalla, p.Descripcion, p.Ruta_Acceso, p.Estado
            );
        }

        public void Eliminar(int id)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_Pantalla_Eliminar @ID_Pantalla={0}", id
            );
        }
    }

}
