using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PagoMoviles.Data;
using PagoMoviles.Helpers;
using PagoMoviles.Models.DTOs;
using PagoMoviles.Models.Entities;
using PagoMoviles.Services.Interfaces;

namespace PagoMoviles.Services.Implementations
{
    /// <summary>
    /// Implementación de los servicios SRV9 (inscripción) y SRV10 (desinscripción) de pagos móviles.
    /// </summary>
    public class PagoMovilService : IPagoMovilService
    {
        private readonly PagosMovilesDbContext _pagosDb;
        private readonly CoreBancarioDbContext _coreDb;
        private readonly IClienteService _clienteService;
        private readonly ILogger<PagoMovilService> _logger;

        public PagoMovilService(
            PagosMovilesDbContext pagosDb,
            CoreBancarioDbContext coreDb,
            IClienteService clienteService,
            ILogger<PagoMovilService> logger)
        {
            _pagosDb = pagosDb;
            _coreDb = coreDb;
            _clienteService = clienteService;
            _logger = logger;
        }

        /// <summary>
        /// SRV9 - Inscripción a pagos móviles.
        /// 
        /// Flujo:
        /// 1. Validar que todos los datos sean obligatorios y válidos.
        /// 2. Verificar que el cliente existe en el core (SRV19).
        /// 3. Verificar que el teléfono no esté asociado a OTRA cuenta activa.
        /// 4. Si ya existe un registro deshabilitado con los mismos datos, reactivarlo.
        /// 5. Si todo está bien, crear el registro de asociación.
        /// </summary>
        public async Task<ApiResponse> InscribirAsync(PagoMovilRegistroDto dto)
        {
            try
            {
                // ─── 1. Validación de datos obligatorios y válidos ───
                if (!ValidationHelper.EsTextoValido(dto.NumeroCuenta) ||
                    !ValidationHelper.EsTextoValido(dto.Identificacion) ||
                    !ValidationHelper.EsTextoValido(dto.NumeroTelefono))
                {
                    return ApiResponse.Error("Datos incorrectos");
                }

                if (!ValidationHelper.EsTelefonoValido(dto.NumeroTelefono))
                {
                    return ApiResponse.Error("Datos incorrectos");
                }

                var telefonoNormalizado = ValidationHelper.NormalizarTelefono(dto.NumeroTelefono!);

                // ─── 2. Verificar que el cliente existe en el core bancario (SRV19) ───
                var clienteExiste = await _clienteService.ClienteExisteAsync(dto.Identificacion!.Trim());
                if (!clienteExiste)
                {
                    return ApiResponse.Error("Datos incorrectos");
                }

                // Verificar que la cuenta exista en el core y pertenezca al cliente
                var cuentaValida = await _coreDb.Cuentas
                    .Include(c => c.Cliente)
                    .AnyAsync(c => c.NumeroCuenta == dto.NumeroCuenta!.Trim()
                                && c.Cliente!.Identificacion == dto.Identificacion!.Trim()
                                && c.Estado);

                if (!cuentaValida)
                {
                    return ApiResponse.Error("Datos incorrectos");
                }

                // ─── 3. Verificar si el teléfono ya está asociado a OTRA cuenta activa ───
                var telefonoAsociado = await _pagosDb.MonederosMoviles
                    .FirstOrDefaultAsync(m => m.NumeroTelefono == telefonoNormalizado && m.Estado);

                if (telefonoAsociado != null)
                {
                    // Si está activo y es de otra cuenta o identificación, rechazar
                    if (telefonoAsociado.NumeroCuenta != dto.NumeroCuenta!.Trim() ||
                        telefonoAsociado.Identificacion != dto.Identificacion!.Trim())
                    {
                        return ApiResponse.Error(
                            "Teléfono ya se encuentra afiliado, realice el proceso de desinscripción");
                    }

                    // Si está activo y es el mismo registro, ya está inscrito
                    return ApiResponse.Error(
                        "Teléfono ya se encuentra afiliado, realice el proceso de desinscripción");
                }

                // ─── 4. Buscar si existe un registro deshabilitado con los mismos datos ───
                var registroExistente = await _pagosDb.MonederosMoviles
                    .FirstOrDefaultAsync(m =>
                        m.NumeroCuenta == dto.NumeroCuenta!.Trim() &&
                        m.Identificacion == dto.Identificacion!.Trim() &&
                        m.NumeroTelefono == telefonoNormalizado &&
                        !m.Estado);

                if (registroExistente != null)
                {
                    // Reactivar el registro existente
                    registroExistente.Estado = true;
                    registroExistente.FechaModificacion = DateTime.Now;
                    _pagosDb.MonederosMoviles.Update(registroExistente);
                    await _pagosDb.SaveChangesAsync();

                    _logger.LogInformation(
                        "SRV9 - Monedero reactivado para teléfono {Telefono}, cuenta {Cuenta}",
                        telefonoNormalizado, dto.NumeroCuenta);

                    return ApiResponse.Ok("Inscripción realizada");
                }

                // ─── 5. Crear nuevo registro de asociación ───
                var nuevoMonedero = new MonederoMovil
                {
                    NumeroCuenta = dto.NumeroCuenta!.Trim(),
                    Identificacion = dto.Identificacion!.Trim(),
                    NumeroTelefono = telefonoNormalizado,
                    Estado = true,
                    FechaCreacion = DateTime.Now
                };

                _pagosDb.MonederosMoviles.Add(nuevoMonedero);
                await _pagosDb.SaveChangesAsync();

                _logger.LogInformation(
                    "SRV9 - Nueva inscripción: teléfono {Telefono}, cuenta {Cuenta}, cliente {Cliente}",
                    telefonoNormalizado, dto.NumeroCuenta, dto.Identificacion);

                return ApiResponse.Ok("Inscripción realizada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SRV9 - Error al inscribir pagos móviles");
                throw;
            }
        }

        /// <summary>
        /// SRV10 - Desinscripción de pagos móviles.
        /// 
        /// Flujo:
        /// 1. Validar que todos los datos sean obligatorios y válidos.
        /// 2. Verificar que el teléfono esté asociado a una cuenta activa.
        /// 3. Deshabilitar el registro (cambiar estado a false).
        /// </summary>
        public async Task<ApiResponse> DesinscribirAsync(PagoMovilRegistroDto dto)
        {
            try
            {
                // ─── 1. Validación de datos obligatorios y válidos ───
                if (!ValidationHelper.EsTextoValido(dto.NumeroCuenta) ||
                    !ValidationHelper.EsTextoValido(dto.Identificacion) ||
                    !ValidationHelper.EsTextoValido(dto.NumeroTelefono))
                {
                    return ApiResponse.Error("Datos incorrectos");
                }

                if (!ValidationHelper.EsTelefonoValido(dto.NumeroTelefono))
                {
                    return ApiResponse.Error("Datos incorrectos");
                }

                var telefonoNormalizado = ValidationHelper.NormalizarTelefono(dto.NumeroTelefono!);

                // ─── 2. Verificar que el teléfono esté asociado a una cuenta activa ───
                var monedero = await _pagosDb.MonederosMoviles
                    .FirstOrDefaultAsync(m =>
                        m.NumeroTelefono == telefonoNormalizado &&
                        m.Estado);

                if (monedero == null)
                {
                    return ApiResponse.Error("Teléfono no se encuentra afiliado");
                }

                // ─── 3. Deshabilitar el registro (manejo por estado) ───
                monedero.Estado = false;
                monedero.FechaModificacion = DateTime.Now;
                _pagosDb.MonederosMoviles.Update(monedero);
                await _pagosDb.SaveChangesAsync();

                _logger.LogInformation(
                    "SRV10 - Desinscripción: teléfono {Telefono}, cuenta {Cuenta}",
                    telefonoNormalizado, monedero.NumeroCuenta);

                return ApiResponse.Ok("Desinscripción realizada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SRV10 - Error al desinscribir pagos móviles");
                throw;
            }
        }
    }
}
