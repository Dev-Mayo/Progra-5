using CoreBancarioService.Model;
using CoreBancarioService.Model.CoreBancarioService.Model;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Repositories
{
    public interface IBitacoraRepository
    {
        Task RegistrarEventoAsync(BitacoraRequest request);
    }
}
