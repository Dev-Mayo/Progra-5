using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Model;

namespace CoreBancarioAPI;

public static class MovimientoEndpoints
{
    public static void MapMovimientoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/core/transaction")
                          .WithTags("SRV14");

        group.MapPost("/", (
            TransactionRequest request,
            ITransactionService service) =>
        {
            try
            {
                var result = service.AplicarTransaccion(request);
                return Results.Created("/core/transaction", result);
            }
            catch (CuentaNoExisteException ex)
            {
                return Results.NotFound(new ErrorResponse
                {
                    Codigo = "404",
                    Mensaje = ex.Message
                });
            }
            catch (SaldoInsuficienteException ex)
            {
                return Results.Conflict(new ErrorResponse
                {
                    Codigo = "402",
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return Results.Problem("Error interno del servidor");
            }
        })
        .WithName("AplicarTransaccion")
        .WithOpenApi();

        var movementsGroup = routes.MapGroup("/core/transactions")
                                   .WithTags("SRV16");

        movementsGroup.MapGet("/", async (
            string identificacion,
            string numeroCuenta,
            IUltimoMovimientoService service
        ) =>
        {
            try
            {
                var movimientos = await service.ConsultarUltimosMovimientos(
                    identificacion,
                    numeroCuenta
                );

                return Results.Ok(movimientos);
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
        .WithName("ConsultarUltimosMovimientos")
        .WithOpenApi();
    }
}
