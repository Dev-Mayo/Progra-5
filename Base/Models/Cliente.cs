namespace AuthService.Models
{
    public class Cliente
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public byte[] ContrasenaHash { get; set; }
        public bool Estado { get; set; }
    }

    // Esta clase servirá para recibir los datos del Login (Header/Body)
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
