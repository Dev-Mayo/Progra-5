using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.DataAccess.Models;
using CoreBancarioService.Model;
using Microsoft.AspNetCore.Mvc;

namespace CoreBancarioAPI;

public static class ClienteCoreEndpoints
{
    public static void MapClienteCoreEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/core/client")
                          .WithTags("SA10");
        group.MapPost("/", (
            ClienteRequest request,
            IClienteService service) =>
        {
            try
            {
                var result = service.CrearCliente(request);
                return Results.Created("/core/client", result);
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
        .WithName("CrearCliente")
        .WithOpenApi();

        group.MapPut("/", (
            ClienteRequest request,
            IClienteService service) =>
        {
            try
            {
                var result = service.EditarCliente(request);
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
        .WithName("EditarCliente")
        .WithOpenApi();

        group.MapDelete("/{identificacion}", (
            string identificacion,
            IClienteService service) =>
        {
            try
            {
                var result = service.EliminarCliente(identificacion);
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
        .WithName("EliminarCliente")
        .WithOpenApi();

        group.MapGet("/", async (
            IClienteService service) =>
        {
            try
            {
                var result = await service.ListarTodos();
                return Results.Ok(result);
            }
            catch (Exception)
            {
                return Results.Problem("Error interno del servidor");
            }
        })
        .WithName("ListarTodos")
        .WithOpenApi();

        group.MapGet("/Cliente/", async (
            string identificacion,
            IClienteService service) =>
        {
            try
            {
                var result = await service.ListarClientePorLlavePrimaria(identificacion);
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
        .WithName("ListarClientePorLlavePrimaria")
        .WithOpenApi();
    }
}
