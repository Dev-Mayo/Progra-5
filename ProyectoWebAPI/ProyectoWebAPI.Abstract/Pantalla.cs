using ProyectoWebAPI.DataAccess.Models;

public interface IPantallaService
{
    Task CrearPantallaAsync(Pantalla pantalla, string usuarioEjecutor, string token);
    Task ModificarPantallaAsync(Pantalla pantalla, string usuarioEjecutor, string token);
    Task EliminarPantallaAsync(int pantallaId, string usuarioEjecutor, string token);
    Task<List<Pantalla>> ObtenerPantallaPorIdAsync(int pantallaId, string usuarioEjecutor, string token);
    Task<List<Pantalla>> ObtenerTodosAsync(string usuarioEjecutor, string token);
}