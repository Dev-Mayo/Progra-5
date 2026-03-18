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
    public class BalanceService : IBalanceService
    {
        private readonly ICuentaRepository _cuentaRepo;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BalanceService(
            ICuentaRepository cuentaRepo,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _cuentaRepo = cuentaRepo;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        public BalanceResponse ConsultarSaldo(BalanceRequest request)
        {
            try
            {
                var saldo = _cuentaRepo.ConsultarSaldo(
                    request.Identificacion,
                    request.NumeroCuenta
                );

                var response = new BalanceResponse
                {
                    NumeroCuenta = request.NumeroCuenta,
                    Saldo = saldo,
                    FechaConsulta = DateTime.Now
                };

                try
                {
                    var usuario = ObtenerUsuarioDesdeToken();

                    var descripcion = $"El usuario consulta el saldo de la cuenta {request.NumeroCuenta}";

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
            catch (SqlException ex)
            {
                throw new CuentaNoExisteException(ex.Message);
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

    }
}
