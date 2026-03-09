using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreBancarioService.Model;
using CoreBancarioService.Model.CoreBancarioService.Model;

namespace CoreBancarioService.Abstract.Repositories
{
    public interface ICuentaRepository
    {
        CuentaModel ObtenerCuentaPorIdentificacion(string identificacionCliente);
        void ActualizarSaldo(int cuentaId, decimal nuevoSaldo);
        decimal ConsultarSaldo(string identificacion, string numeroCuenta);
        void CrearCuenta(int clienteId, string tipoCuenta);
        void EditarCuenta(int clienteId, string numeroCuenta, string tipoCuenta);
        void EliminarCuenta(int clienteId, string numeroCuenta);
        Task<IEnumerable<CuentaResponse>> ListarTodas();
        Task<IEnumerable<CuentaResponse>> ListarPorLlavePrimaria(string numeroCuenta);
        Task<IEnumerable<CuentaResponse>> ListarPorCliente(int clienteId);
    }
}
