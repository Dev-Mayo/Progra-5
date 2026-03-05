using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Model;

namespace CoreBancarioAPI;

public static class CuentaCoreEndpoints
{
    public static void MapCuentaCoreEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/core/accounts")
                          .WithTags("SA11");
        group.MapPost("/", (
            CuentaRequest request,
            ICuentaService service) =>
        {
            try
            {
                var result = service.CrearCuenta(request);//funciona
                return Results.Created("/core/accounts", result);
            }
            catch (ClienteNoExiste ex)
            {
                return Results.NotFound(new ErrorResponse
                {
                    Codigo = "404",
                    Mensaje = ex.Message
                });
            }
            catch (TipoCuentaInvalido ex)
            {
                return Results.Conflict(new ErrorResponse
                {
                    Codigo = "409",
                    Mensaje = ex.Message
                });
            }
            catch (CuentaYaExiste ex)
            {
                return Results.Conflict(new ErrorResponse
                {
                    Codigo = "409",
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return Results.Problem("Error interno del servidor");
            }
        })
        .WithName("CrearCuenta")
        .WithOpenApi();
    }
}
