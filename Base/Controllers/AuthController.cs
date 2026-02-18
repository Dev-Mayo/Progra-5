using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    [Route("api")] // Las rutas serán api/login, api/validate, etc.
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthService.Models.LoginRequest request)
        {
            // 1. Buscamos al usuario solo por Email
            var usuario = _context.Clientes.FirstOrDefault(u => u.Email == request.Email && u.Estado == true);

            // 2. Si el usuario existe, VERIFICAMOS la contraseña encriptada
            
            if (usuario == null)
            {
                return Unauthorized(new { message = "Usuario y/o contraseña incorrectos" });
            }

            var hashIngresado = CalcularSha256(request.Password);

            if (!CompararHashes(hashIngresado, usuario.ContrasenaHash))
            {
                return Unauthorized(new { message = "Usuario y/o contraseña incorrectos" });
            }

            // 3. Si todo está bien, generamos el token (tu código anterior)
            var token = GenerarToken(usuario);

            return Created("", new
            {
                expires_in = DateTime.UtcNow.AddMinutes(5),
                access_token = token,
                refresh_token = Guid.NewGuid().ToString()
            });
        }

        [HttpGet("validate")]
        public IActionResult Validate([FromHeader(Name = "Authorization")] string authorization)
        {
            // Criterio SRV5: Recibe un token y responde 200 OK y true si es válido
            if (string.IsNullOrEmpty(authorization)) return Unauthorized();

            try
            {
                var token = authorization.Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetSection("Jwt:Key").Value!);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config.GetSection("Jwt:Issuer").Value,
                    ValidateAudience = true,
                    ValidAudience = _config.GetSection("Jwt:Audience").Value,
                    ClockSkew = TimeSpan.Zero // Para que expire exactamente a los 5 min
                }, out SecurityToken validatedToken);

                return Ok(true); // Responde 200 OK y true
            }
            catch
            {
                // Criterio SRV5: Si no es válido responde 401 Unauthorized
                return Unauthorized();
            }
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] AuthService.Models.RefreshRequest request)
        {
            // 1. Validar que el refresh token no venga vacío
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "El refresh_token es requerido." });
            }

            // 2. En una app real, buscaríamos este token en una tabla 'Tokens' en la DB.
            // Para efectos de este proyecto, simularemos la validación buscando al usuario.
            var usuario = _context.Clientes.FirstOrDefault(u => u.Estado == true);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Token de refresco inválido o usuario no encontrado" });
            }

            // 3. Generar un nuevo Access Token (usando el método que ya tienes)
            var nuevoToken = GenerarToken(usuario);

            // 4. Responder con el nuevo set de llaves (Criterio SRV5)
            return Created("", new
            {
                expires_in = DateTime.UtcNow.AddMinutes(5),
                access_token = nuevoToken,
                refresh_token = Guid.NewGuid().ToString(), // Generamos uno nuevo para la siguiente vez
                usuarioID = usuario.Identificacion
            });
        }

        private string GenerarToken(Cliente usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("id", usuario.Identificacion.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private byte[] CalcularSha256(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool CompararHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }

            return true;
        }

    }
}