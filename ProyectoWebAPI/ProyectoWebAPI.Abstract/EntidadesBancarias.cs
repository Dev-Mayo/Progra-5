using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.Abstract
{
    public interface IEntidadesService
    {
        Task CrearEntidadAsync(EntidadesBancarias entidad, string usuarioEjecutor, string token);
        Task ModificarEntidadAsync(EntidadesBancarias entidad, string usuarioEjecutor, string token);
        Task EliminarEntidadAsync(int entidadId, string usuarioEjecutor, string token);
        Task<List<EntidadesBancarias>> ObtenerEntidadPorIdAsync(int entidadId, string usuarioEjecutor, string token);
        Task<List<EntidadesBancarias>> ObtenerTodosAsync(string usuarioEjecutor, string token);
    }
}