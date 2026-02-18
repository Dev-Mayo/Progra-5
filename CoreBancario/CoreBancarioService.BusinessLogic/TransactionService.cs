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
    public class TransactionService : ITransactionService
    {
        private readonly ICuentaRepository _cuentaRepo;
        private readonly IMovimientoRepository _movimientoRepo;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TransactionService(
            ICuentaRepository cuentaRepo,
            IMovimientoRepository movimientoRepo,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _cuentaRepo = cuentaRepo;
            _movimientoRepo = movimientoRepo;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }


        public TransactionResponse AplicarTransaccion(TransactionRequest request)
        {
            try
            {
                _movimientoRepo.AplicarTransaccion(
                    request.NumeroCuenta,
                    request.TipoMovimiento,
                    request.Monto,
                    request.Descripcion
                );
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50001)
                    throw new CuentaNoExisteException(ex.Message);

                if (ex.Number == 50003)
                    throw new SaldoInsuficienteException(ex.Message);

                throw;
            }

            var response = new TransactionResponse
            {
                NumeroCuenta = request.NumeroCuenta,
                TipoMovimiento = request.TipoMovimiento,
                Monto = request.Monto,
                Fecha = DateTime.Now
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
