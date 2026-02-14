using Microsoft.EntityFrameworkCore;
using PagoMoviles.Data;
using PagoMoviles.Services.Interfaces;

namespace PagoMoviles.Services.Implementations
{
    /// <summary>
    /// SRV19 - Servicio para verificar si un cliente existe en el core bancario.
    /// Consulta directamente la tabla 'cliente' de la BD del core bancario.
    /// Endpoint: /core/client-exists
    /// </summary>
    public class ClienteService : IClienteService
    {
        private readonly CoreBancarioDbContext _coreDb;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(CoreBancarioDbContext coreDb, ILogger<ClienteService> logger)
        {
            _coreDb = coreDb;
            _logger = logger;
        }

        /// <summary>
        /// Verifica si un cliente existe en el core bancario.
        /// Busca por identificación y que esté activo (estado = 1).
        /// </summary>
        public async Task<bool> ClienteExisteAsync(string identificacion)
        {
            try
            {
                var existe = await _coreDb.Clientes
                    .AnyAsync(c => c.Identificacion == identificacion && c.Estado);

                _logger.LogInformation(
                    "SRV19 - Consulta cliente con identificación {Identificacion}: {Resultado}",
                    identificacion, existe ? "EXISTE" : "NO EXISTE");

                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "SRV19 - Error al consultar cliente con identificación {Identificacion}", 
                    identificacion);
                throw;
            }
        }
    }
}
