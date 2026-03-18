using System.Threading.Tasks;

namespace ProyectoWebAPI.Abstract
{
    public interface IBitacoraClient
    {
        Task RegistrarAsync(string usuario, string descripcion, string token); // ← Agregar token
    }
}