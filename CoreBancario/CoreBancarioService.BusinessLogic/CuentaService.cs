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
    public class CuentaService : ICuentaService //falta agregar toda la logica
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

                var descripcion = $"Nuevo registro realizado: {JsonSerializer.Serialize(response)}";

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
            throw new NotImplementedException();
        }
        public CuentaResponse EliminarCuenta(CuentaRequest request)
        {
            throw new NotImplementedException();
        }
        public CuentaResponse ListarTodas(CuentaRequest request)
        {
            throw new NotImplementedException();
        }
        public CuentaResponse ListarPorLlavePrimaria(CuentaRequest request)
        {
            throw new NotImplementedException();
        }
        public CuentaResponse ListarPorCliente(CuentaRequest request)
        {
            throw new NotImplementedException();
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
    }
}
