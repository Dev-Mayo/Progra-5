using CoreBancarioService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Services
{
    public interface IClienteService
    {
        ClienteResponse CrearCliente(ClienteRequest request);
        Task<ClienteResponse> EditarCliente(ClienteRequestEdit request);
        ClienteResponse EliminarCliente(string identificacion);
        Task<IEnumerable<ClienteResponse>> ListarTodos();
        Task<IEnumerable<ClienteResponse>> ListarClientePorLlavePrimaria(string identificacion);
    }
}
