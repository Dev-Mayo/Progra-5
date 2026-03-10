using Azure.Core;
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
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepo;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClienteService(
            IClienteRepository clienteRepo,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _clienteRepo = clienteRepo;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }
        public ClienteResponse CrearCliente(ClienteRequest request)
        {
            try
            {
                _clienteRepo.CrearCliente(
                    request.identificacion,
                    request.nombre,
                    request.apellido,
                    request.fecha_nacimiento,
                    request.TipoIdentificacion,
                    request.Telefono,
                    request.Email,
                    request.Contrasena
                );
            }
            catch (SqlException ex)
            {

                if (ex.Number == 60071)
                    throw new FechaNacimientoIncorrecta(ex.Message);

                if (ex.Number == 60072)
                    throw new ClienteYaExiste(ex.Message);

                throw;
            }

            var response = new ClienteResponse
            {
                identificacion = request.identificacion,
                nombre = request.nombre,
                apellido = request.apellido,
                fecha_nacimiento = request.fecha_nacimiento,
                TipoIdentificacion = request.TipoIdentificacion,
                Telefono = request.Telefono
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

        public ClienteResponse EditarCliente(ClienteRequestEdit request)
        {
            try
            {
                _clienteRepo.EditarCliente(
                    request.identificacion,
                    request.nombre,
                    request.apellido,
                    request.fecha_nacimiento,
                    request.TipoIdentificacion,
                    request.Telefono,
                    request.Email,
                    request.Contrasena
                );
            }
            catch (SqlException ex)
            {
                if (ex.Number == 60081)
                    throw new ClienteNoExiste(ex.Message);
                if (ex.Number == 60082)
                    throw new FechaNacimientoIncorrecta(ex.Message);
                throw;
            }

            var response = new ClienteResponse
            {
                identificacion = request.identificacion,
                nombre = request.nombre,
                apellido = request.apellido,
                fecha_nacimiento = request.fecha_nacimiento,
                TipoIdentificacion = request.TipoIdentificacion,
                Telefono = request.Telefono
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

        public ClienteResponse EliminarCliente(string identificacion)
        {
            try
            {
                _clienteRepo.EliminarCliente(
                    identificacion
                );
            }
            catch (SqlException ex)
            {
                if (ex.Number == 60091)
                    throw new ClienteNoExiste(ex.Message);

                if (ex.Number == 60092)
                    throw new ClienteTieneCuentasRegistradas(ex.Message);

                throw;
            }

            var response = new ClienteResponse
            {
                identificacion = identificacion
            };


            try
            {
                var usuario = ObtenerUsuarioDesdeToken();

                var descripcion = $"Cliente eliminada: {JsonSerializer.Serialize(response)}";

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
        public async Task<IEnumerable<ClienteResponse>> ListarTodos()
        {
            try
            {
                var clientes = await _clienteRepo.ListarTodos();
                await RegistrarEventoBitacora($"Usuario consulta los clientes activos");
                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en ListarTodos: " + ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<ClienteResponse>> ListarClientePorLlavePrimaria(string identificacion)
        {
            try
            {
                var clientes = await _clienteRepo.ListarClientePorLlavePrimaria(identificacion);
                await RegistrarEventoBitacora($"Usuario consulta el cliente {identificacion}");

                return clientes;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 70011)
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
