using CoreBancarioService.Abstract.Security;

namespace CoreBancarioAPI.Validation
{
    public class TokenValidation
    {
        private readonly RequestDelegate _next;

        public TokenValidation(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ITokenValidationService tokenService)
        {
            if (context.Request.Path.StartsWithSegments("/core"))
            {
                var authHeader = context.Request.Headers["Authorization"]
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader) ||
                    !authHeader.StartsWith("Bearer "))
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var isValid = await tokenService.ValidateTokenAsync(token);

                if (!isValid)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }

            await _next(context);
        }
    }
}
