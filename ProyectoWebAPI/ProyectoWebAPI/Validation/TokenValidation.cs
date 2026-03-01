using ProyectoWebAPI.Abstract;

namespace ProyectoWebAPI.Validation
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
            ITokenService tokenService)
        {
         
            if (context.Request.Path.StartsWithSegments("/user") ||
                context.Request.Path.StartsWithSegments("/screen") ||
                context.Request.Path.StartsWithSegments("/rol") ||
                context.Request.Path.StartsWithSegments("/entidad"))
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        mensaje = "Token requerido"
                    });
                    return;
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var esValido = await tokenService.ValidarTokenAsync(token);

                if (!esValido)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        mensaje = "Token inválido"
                    });
                    return;
                }

               
                context.Items["Token"] = token;
                context.Items["Usuario"] = await tokenService.ObtenerUsuarioDelTokenAsync(token);
            }

            await _next(context);
        }
    }
}