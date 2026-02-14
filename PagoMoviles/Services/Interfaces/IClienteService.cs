namespace PagoMoviles.Services.Interfaces
{
    /// <summary>
    /// Interface para el servicio SRV19 - Verificar si un cliente existe en el core bancario.
    /// </summary>
    public interface IClienteService
    {
        /// <summary>
        /// Verifica si un cliente existe en el core bancario por su identificación.
        /// </summary>
        /// <param name="identificacion">Número de identificación del cliente.</param>
        /// <returns>True si el cliente existe y está activo.</returns>
        Task<bool> ClienteExisteAsync(string identificacion);
    }
}
