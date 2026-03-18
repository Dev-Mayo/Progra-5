using TareaCorta2.DTO;
using TareaCorta2.Model;

namespace TareaCorta2.Interface
{
    public interface IClienteService
    {
        Task<ApiResponse<Cliente>> CrearAsync(ClienteCreateDto dto);
        Task<ApiResponse<List<Cliente>>> ObtenerTodosAsync();
        Task<ApiResponse<Cliente>> ObtenerPorIdAsync(string id);
    }
}
