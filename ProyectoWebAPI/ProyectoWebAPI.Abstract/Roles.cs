using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.Abstract
{
    public interface IRolService
    {
        Task CrearRolAsync(Roles rol, string usuarioEjecutor, string token);
        Task ModificarRolAsync(Roles rol, string usuarioEjecutor, string token);
        Task EliminarRolAsync(int rolId, string usuarioEjecutor, string token);
        Task<List<Roles>> ObtenerRolPorIdAsync(int rolId, string usuarioEjecutor, string token  );
        Task<List<Roles>> ObtenerTodosAsync(string usuarioEjecutor, string token);
    }
}