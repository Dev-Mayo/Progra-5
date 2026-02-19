using ConfigService.Data;
using ConfigService.Services;

namespace ConfigService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers y Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ======== NUESTROS SERVICIOS (DI) ========
            builder.Services.AddSingleton<ConfigDb>();

            // HttpClient para AuthService (/validate) y AuditService (/bitacora)
            builder.Services.AddHttpClient<AuthValidatorClient>(client =>
            {
                // Se configura en appsettings.json (Auth:BaseUrl)
            });
            builder.Services.AddHttpClient<BitacoraClient>(client =>
            {
                // Se configura en appsettings.json (Audit:BaseUrl)
                client.Timeout = TimeSpan.FromSeconds(3); // que no bloquee el request
            });

            builder.Services.AddSingleton<JwtReader>(); // opcional para extraer uid del JWT

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // No usamos [Authorize] aquí: SRV4 exige validar llamando a /validate del AuthService
            app.MapControllers();

            // Healthcheck
            app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "ConfigService", utc = DateTimeOffset.UtcNow }));

            app.Run();
        }
    }
}