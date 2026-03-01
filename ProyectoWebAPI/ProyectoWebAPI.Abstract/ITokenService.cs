using System.Threading.Tasks;

namespace ProyectoWebAPI.Abstract
{
    public interface ITokenService
    {
        Task<bool> ValidarTokenAsync(string token);
        Task<string> ObtenerUsuarioDelTokenAsync(string token);
    }
}