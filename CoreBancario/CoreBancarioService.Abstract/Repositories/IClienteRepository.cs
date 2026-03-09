using CoreBancarioService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Repositories
{
    public interface IClienteRepository
    {
        void CrearCliente(string identificacion, string nombre, string apellido, 
            DateOnly fecha_nacimiento, int TipoIdentificacion, int Telefono);
        void EditarCliente(string identificacion, string nombre, string apellido,
            DateOnly fecha_nacimiento, int TipoIdentificacion, int Telefono);
        void EliminarCliente(string identificacion);
        Task<IEnumerable<ClienteResponse>> ListarTodos();
        Task<IEnumerable<ClienteResponse>> ListarClientePorLlavePrimaria(string identificacion);
    }
}
