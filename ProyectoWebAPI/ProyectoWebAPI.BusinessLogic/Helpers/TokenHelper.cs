using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProyectoWebAPI.BusinessLogic.Helpers
{
    public static class TokenHelper
    {
        public static string ObtenerUsuarioDelToken(string token)
        {
            try
            {
                token = token?.Replace("Bearer ", "").Trim();
                if (string.IsNullOrWhiteSpace(token))
                    return "sistema";

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var usuario = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Name ||
                                        c.Type == "unique_name" ||
                                        c.Type == "email")?.Value;

                return usuario ?? "sistema";
            }
            catch
            {
                return "sistema";
            }
        }
    }
}