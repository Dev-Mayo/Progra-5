using Azure.Core;
using CoreBancarioService.Abstract.Bitacora;
using CoreBancarioService.Abstract.Bitacora.Bitacora;
using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Model;
using CoreBancarioService.Model.CoreBancarioService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace CoreBancarioService.BusinessLogic
{
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _cuentaRepo;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CuentaService(
            ICuentaRepository cuentaRepo,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _cuentaRepo = cuentaRepo;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        public CuentaResponse CrearCuenta(CuentaRequest request)
        {
            try
            {
                _cuentaRepo.CrearCuenta(
                    request.ClienteId,
                    request.TipoCuenta
                );
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50070)
                    throw new ClienteNoExiste(ex.Message);

                if (ex.Number == 50071)
                    throw new TipoCuentaInvalido(ex.Message);

                if (ex.Number == 50072)
                    throw new CuentaYaExiste(ex.Message);

                throw;
            }

            var response = new CuentaResponse
            {
                ClienteId = request.ClienteId,
                //NumeroCuenta = request.NumeroCuenta, -- implementar un select desde BD
                TipoCuenta = request.TipoCuenta,
                Estado = true,
                FechaCreacion = DateTime.Now
            };


            try
            {
                var usuario = ObtenerUsuarioDesdeToken();

                var descripcion = $"Nueva cuenta registrada: {JsonSerializer.Serialize(response)}";

                _bitacoraService.RegistrarEventoAsync(new BitacoraRequest
                {
                    UsuarioAccion = usuario,
                    Descripcion = descripcion
                }).Wait();
            }
            catch
            {
            }

            return response;
        }
        public CuentaResponse EditarCuenta(CuentaRequest request) 
        {
            try
            {
                _cuentaRepo.EditarCuenta(
                    request.ClienteId,
                    request.NumeroCuenta,
                    request.TipoCuenta
                );
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50030)
                    throw new TipoCuentaInvalido(ex.Message);

                if (ex.Number == 50031)
                    throw new CuentaNoExisteException(ex.Message);

                throw;
            }

            var response = new CuentaResponse
            {
                ClienteId = request.ClienteId,
                NumeroCuenta = request.NumeroCuenta,
                TipoCuenta = request.TipoCuenta,
                Estado = true,
                FechaCreacion = DateTime.Now
            };


            try
            {
                var usuario = ObtenerUsuarioDesdeToken();

                var descripcion = $"Cuenta editada: {JsonSerializer.Serialize(response)}";

                _bitacoraService.RegistrarEventoAsync(new BitacoraRequest
                {
                    UsuarioAccion = usuario,
                    Descripcion = descripcion
                }).Wait();
            }
            catch
            {
            }

            return response;
        }
        public CuentaResponse EliminarCuenta(
            int ClienteId,
            string NumeroCuenta)
        {
            try
            {
                _cuentaRepo.EliminarCuenta(
                    ClienteId,
                    NumeroCuenta
                );
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50040)
                    throw new CuentaNoExisteException(ex.Message);

                if (ex.Number == 50041)
                    throw new CuentaTieneMovimientosRegistradosException(ex.Message);

                throw;
            }

            var response = new CuentaResponse
            {
                ClienteId = ClienteId,
                NumeroCuenta = NumeroCuenta,
                FechaCreacion = DateTime.Now
            };


            try
            {
                var usuario = ObtenerUsuarioDesdeToken();

                var descripcion = $"Cuenta eliminada: {JsonSerializer.Serialize(response)}";

                _bitacoraService.RegistrarEventoAsync(new BitacoraRequest
                {
                    UsuarioAccion = usuario,
                    Descripcion = descripcion
                }).Wait();
            }
            catch
            {
            }

            return response;
        }
        public async Task<IEnumerable<CuentaResponse>> ListarTodas()
        {
            try
            {
                var cuentas = await _cuentaRepo.ListarTodas();
                await RegistrarEventoBitacora($"Usuario consulta las cuentas bancarias activas");
                return cuentas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en ListarTodas: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CuentaResponse>> ListarPorLlavePrimaria(string numeroCuenta)
        {
            try
            {
                var cuentas = await _cuentaRepo.ListarPorLlavePrimaria(numeroCuenta);
                await RegistrarEventoBitacora($"Usuario consulta la cuenta {numeroCuenta}");
 
                return cuentas;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50071)
                    throw new CuentaNoExisteException(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CuentaResponse>> ListarPorCliente(int ClienteID)
        {
            try
            {
                var cuentas = await _cuentaRepo.ListarPorCliente(ClienteID);
                await RegistrarEventoBitacora($"Usuario consulta las cuentas del cliente: {ClienteID}");
                return cuentas;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50081)
                    throw new ClienteNoExiste(ex.Message);
             
                if (ex.Number == 50060)
                    throw new ClienteNoExiste(ex.Message);
                throw;
            }
        }

        private string ObtenerUsuarioDesdeToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authHeader = httpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader))
                return "Desconocido";

            var token = authHeader.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email");

            return emailClaim?.Value ?? "Desconocido";
        }

        public async Task RegistrarEventoBitacora(string descripcion)
        {
            try
            {
                var usuario = ObtenerUsuarioDesdeToken();

                _bitacoraService.RegistrarEventoAsync(new BitacoraRequest
                {
                    UsuarioAccion = usuario,
                    Descripcion = descripcion
                }).Wait();
            }
            catch
            {
            }
        }
    }
}
