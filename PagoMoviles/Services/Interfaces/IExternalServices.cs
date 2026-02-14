namespace PagoMoviles.Services.Interfaces
{
    /// <summary>
    /// Interface para el servicio de validación de tokens (SRV5).
    /// Implementada por otro compañero. Usamos mock mientras tanto.
    /// Cuando la implementación real esté lista, solo se cambia en Program.cs.
    /// </summary>
    public interface ITokenValidationService
    {
        /// <summary>
        /// Valida si un token JWT es válido.
        /// Corresponde al endpoint /validate de SRV5.
        /// </summary>
        /// <param name="token">Token JWT a validar.</param>
        /// <returns>True si el token es válido, false si no.</returns>
        Task<bool> ValidateTokenAsync(string token);
    }

    /// <summary>
    /// Interface para el servicio de bitácoras (SRV18).
    /// Implementada por otro compañero. Usamos mock mientras tanto.
    /// </summary>
    public interface IBitacoraService
    {
        /// <summary>
        /// Registra una acción en la bitácora.
        /// Corresponde al endpoint /bitacora de SRV18.
        /// </summary>
        /// <param name="usuario">Usuario que ejecuta la acción.</param>
        /// <param name="descripcion">Descripción de la acción (Acción + JSON).</param>
        /// <param name="token">Token de autorización para el servicio de bitácora.</param>
        Task RegistrarBitacoraAsync(string usuario, string descripcion, string token);
    }
}
