using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Model;

namespace CoreBancarioAPI;

public static class CuentaEndpoints
{
    public static void MapCuentaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/core/balance")
                          .WithTags("SRV15");

        group.MapGet("/", (
            string identificacion,
            string numeroCuenta,
            //string authorization,
            IBalanceService balanceService
            //IAuthService authService
            ) =>
        {
            // if (!authService.Validate(authorization))
            //return Results.Unauthorized(); -- necesito el API de autenticacion
            Console.WriteLine($"Identificacion: {identificacion}");
            Console.WriteLine($"NumeroCuenta: {numeroCuenta}");
            try
            {
                var saldo = balanceService.ConsultarSaldo(new BalanceRequest
                {
                    Identificacion = identificacion,
                    NumeroCuenta = numeroCuenta
                });

                return Results.Ok(new
                {
                    saldo
                });
            }
            catch (CuentaNoExisteException ex)
            {
                return Results.NotFound(new ErrorResponse
                {
                    Codigo = "404",
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return Results.Problem(
                    title: "Error interno",
                    statusCode: 500
                );
            }
        })
        .WithName("ConsultarSaldo")
        .WithOpenApi();
    }
}
