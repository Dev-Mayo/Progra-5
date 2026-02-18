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

namespace CoreBancarioService.BusinessLogic
{
    public class UltimoMovimientoService : IUltimoMovimientoService
    {
        private readonly IUltimoMovimientoRepository _movimientoRepo;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UltimoMovimientoService(
            IUltimoMovimientoRepository movimientoRepo,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _movimientoRepo = movimientoRepo;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IEnumerable<MovimientoResponse>> ConsultarUltimosMovimientos(
            string identificacion,
            string numeroCuenta)
        {
            try
            {
                var movimientos = await _movimientoRepo
                    .ConsultarUltimosMovimientos(identificacion, numeroCuenta);

                try
                {
                    var usuario = ObtenerUsuarioDesdeToken();

                    var descripcion = $"El usuario consulta los últimos movimientos de la cuenta {numeroCuenta}";

                    await _bitacoraService.RegistrarEventoAsync(new BitacoraRequest
                    {
                        UsuarioAccion = usuario,
                        Descripcion = descripcion
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error registrando bitácora: " + ex.Message);
                }

                return movimientos;
            }
            catch (SqlException ex) when (ex.Number == 50020)
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
