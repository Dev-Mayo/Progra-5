using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Model;
using Microsoft.AspNetCore.Mvc;

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

        group.MapPut("/", (
            CuentaRequest request,
            ICuentaService service) =>
        {
            try
            {
                var result = service.EditarCuenta(request);
                return Results.NoContent();
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
                return Results.BadRequest(new ErrorResponse
                {
                    Codigo = "400",
                    Mensaje = ex.Message
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
                return Results.Problem("Error interno del servidor");
            }
        })
        .WithName("EditarCuenta")
        .WithOpenApi();

        group.MapDelete("/{ClienteId:int}/{NumeroCuenta}", (
            int ClienteId,
            string NumeroCuenta,
            ICuentaService service) =>
        {
            try
            {
                var result = service.EliminarCuenta(ClienteId, NumeroCuenta);
                return Results.NoContent();
            }
            catch (CuentaTieneMovimientosRegistradosException ex)
            {
                return Results.Conflict(new ErrorResponse
                {
                    Codigo = "409",
                    Mensaje = ex.Message
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
            catch (Exception ex)
            {
                return Results.Problem(
                    statusCode: 500,
                    title: "Error interno del servidor al eliminar la cuenta"
                );
            }
        })
        .WithName("EliminarCuenta")
        .WithOpenApi();

        group.MapGet("/", async (
            ICuentaService service) =>
        {
            try
            {
                var result = await service.ListarTodas();
                return Results.Ok(result);
            }
            catch (Exception)
            {
                return Results.Problem("Error interno del servidor");
            }
        })
        .WithName("ListarTodas")
        .WithOpenApi();

        group.MapGet("/Cuenta/", async (
            string NumeroCuenta,
            ICuentaService service) =>
        {
            try
            {
                var result = await service.ListarPorLlavePrimaria(NumeroCuenta);
                return Results.Ok(result);
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
                return Results.Problem("Error interno del servidor");
            }
        })
        .WithName("ListarPorLlavePrimaria")
        .WithOpenApi();

        group.MapGet("/Cliente/", async (
            int ClienteID,
            ICuentaService service) =>
        {
            try
            {
                var result = await service.ListarPorCliente(ClienteID);
                return Results.Ok(result);
            }
            catch (ClienteNoExiste ex)
            {
                return Results.NotFound(new ErrorResponse
                {
                    Codigo = "404",
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return Results.Problem("Error interno del servidor");
            }
        })
        .WithName("ListarPorCliente")
        .WithOpenApi();
    }
}
