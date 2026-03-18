using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess;
using ProyectoWebAPI.DataAccess.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProyectoWebAPI.BusinessLogic
{
    public class ClienteService : IClienteService
    {
        private readonly ClienteRepository _repo;
        private readonly IBitacoraClient _bitacoraClient;

        public ClienteService(ClienteRepository repo, IBitacoraClient bitacoraClient)
        {
            _repo = repo;
            _bitacoraClient = bitacoraClient;
        }

        // ========================= CREAR =========================

        public async Task CrearClienteAsync(Cliente cliente, string usuarioEjecutor, string token) // ← Agregar token
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cliente.Password))
                    throw new Exception("Password requerido");

                Validar(cliente);

                cliente.ContrasenaHash = Hash(cliente.Password);

                _repo.Insertar(cliente);

                string jsonNuevo = JsonSerializer.Serialize(cliente);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se creó el cliente: {jsonNuevo}",
                    token // ← Agregar token
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al crear cliente: {ex.Message}",
                    token // ← Agregar token
                );
                throw;
            }
        }

        // ========================= MODIFICAR =========================

        public async Task ModificarClienteAsync(Cliente cliente, string usuarioEjecutor, string token) // ← Agregar token
        {
            try
            {
                if (cliente.cliente_id <= 0)
                    throw new Exception("Id inválido");

                var clienteAnterior = _repo.ObtenerPorId(cliente.cliente_id).FirstOrDefault();
                if (clienteAnterior == null)
                    throw new Exception("Cliente no existe");

                string jsonAnterior = JsonSerializer.Serialize(clienteAnterior);

                Validar(cliente);

                if (!string.IsNullOrWhiteSpace(cliente.Password))
                {
                    cliente.ContrasenaHash = Hash(cliente.Password);
                }
                else
                {
                    cliente.ContrasenaHash = clienteAnterior.ContrasenaHash;
                }

                _repo.Modificar(cliente);

                string jsonNuevo = JsonSerializer.Serialize(cliente);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se modificó el cliente.\nANTES: {jsonAnterior}\nDESPUÉS: {jsonNuevo}",
                    token // ← Agregar token
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al modificar cliente: {ex.Message}",
                    token // ← Agregar token
                );
                throw;
            }
        }

        // ========================= ELIMINAR =========================

        public async Task EliminarClienteAsync(int clienteId, string usuarioEjecutor, string token) // ← Agregar token
        {
            try
            {
                var cliente = _repo.ObtenerPorId(clienteId).FirstOrDefault();
                if (cliente == null)
                    throw new Exception("Cliente no existe");

                string jsonEliminado = JsonSerializer.Serialize(cliente);

                _repo.Eliminar(clienteId);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se eliminó el cliente: {jsonEliminado}",
                    token // ← Agregar token
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al eliminar cliente: {ex.Message}",
                    token // ← Agregar token
                );
                throw;
            }
        }

        // ========================= OBTENER POR ID =========================

        public async Task<List<Cliente>> ObtenerClientePorIdAsync(int clienteId, string usuarioEjecutor, string token) // ← Agregar token
        {
            var resultado = _repo.ObtenerPorId(clienteId);

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                $"El usuario consultó el cliente con ID {clienteId}",
                token // ← Agregar token
            );

            return resultado;
        }

        // ========================= OBTENER TODOS =========================

        public async Task<List<Cliente>> ObtenerTodosAsync(string usuarioEjecutor, string token) // ← Agregar token
        {
            var resultado = _repo.ObtenerTodos();

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                $"El usuario consultó todos los clientes",
                token // ← Agregar token
            );

            return resultado;
        }

        // ========================= FILTRAR =========================

        public async Task<List<Cliente>> FiltrarAsync(string id, string nombre, string tipo, string usuarioEjecutor, string token) // ← Agregar token
        {
            var resultado = _repo.Filtrar(id, nombre, tipo);

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                $"El usuario realizó filtro de clientes",
                token // ← Agregar token
            );

            return resultado;
        }

        // ========================= VALIDACIONES =========================

        private void Validar(Cliente c)
        {
            if (string.IsNullOrWhiteSpace(c.nombre))
                throw new Exception("Nombre requerido");

            if (string.IsNullOrWhiteSpace(c.apellido))
                throw new Exception("Apellido requerido");

            if (!Regex.IsMatch(c.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Email inválido");
        }

        private byte[] Hash(string input)
        {
            using SHA256 sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}