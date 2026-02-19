using System.IdentityModel.Tokens.Jwt;

namespace ConfigService.Services
{
    public class JwtReader
    {
        public string? GetUserIdFromRequest(HttpRequest request)
        {
            var auth = request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(auth) || !auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return null;

            var token = auth["Bearer ".Length..].Trim();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var uid = jwt.Claims.FirstOrDefault(c => c.Type == "uid")?.Value
                       ?? jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                return uid;
            }
            catch
            {
                return null;
            }
        }
    }
}