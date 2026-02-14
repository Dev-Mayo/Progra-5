using PagoMoviles.Models.DTOs;

namespace PagoMoviles.Services.Interfaces
{
    /// <summary>
    /// Interface para los servicios de inscripción (SRV9) y desinscripción (SRV10) de pagos móviles.
    /// </summary>
    public interface IPagoMovilService
    {
        /// <summary>
        /// SRV9 - Inscribe un cliente al servicio de pagos móviles.
        /// Asocia un número de teléfono a una cuenta bancaria.
        /// </summary>
        Task<ApiResponse> InscribirAsync(PagoMovilRegistroDto dto);

        /// <summary>
        /// SRV10 - Desinscribe un cliente del servicio de pagos móviles.
        /// Deshabilita la asociación del teléfono con la cuenta.
        /// </summary>
        Task<ApiResponse> DesinscribirAsync(PagoMovilRegistroDto dto);
    }
}
