public interface IClienteService
{
    Task CrearClienteAsync(Cliente cliente, string usuarioEjecutor, string token);
    Task ModificarClienteAsync(Cliente cliente, string usuarioEjecutor, string token);
    Task EliminarClienteAsync(int clienteId, string usuarioEjecutor, string token);
    Task<List<Cliente>> ObtenerClientePorIdAsync(int clienteId, string usuarioEjecutor, string token);
    Task<List<Cliente>> ObtenerTodosAsync(string usuarioEjecutor, string token);
    Task<List<Cliente>> FiltrarAsync(string id, string nombre, string tipo, string usuarioEjecutor, string token);
}