namespace PagoMoviles.Abstract
{
    public interface IBitacoraService
    {
        // Añadimos token
        Task RegistrarAsync(string UsuarioAccion, string Descripcion, string token);
    }
}