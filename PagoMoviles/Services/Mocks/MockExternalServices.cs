using PagoMoviles.Services.Interfaces;

namespace PagoMoviles.Services.Mocks
{
    /// <summary>
    /// Mock del servicio de validación de tokens (SRV5).
    /// TEMPORAL: Siempre retorna true para poder trabajar sin depender del compañero.
    /// 
    /// CUANDO EL COMPAÑERO TERMINE SRV5:
    /// 1. Crear una clase que implemente ITokenValidationService
    ///    y haga un HTTP call real al endpoint /validate.
    /// 2. En Program.cs cambiar:
    ///    builder.Services.AddScoped<ITokenValidationService, MockTokenValidationService>();
    ///    POR:
    ///    builder.Services.AddScoped<ITokenValidationService, RealTokenValidationService>();
    /// </summary>
    public class MockTokenValidationService : ITokenValidationService
    {
        private readonly ILogger<MockTokenValidationService> _logger;

        public MockTokenValidationService(ILogger<MockTokenValidationService> logger)
        {
            _logger = logger;
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            _logger.LogWarning("[MOCK] Validación de token simulada. Token recibido: {Token}", 
                token?.Substring(0, Math.Min(20, token?.Length ?? 0)) + "...");
            
            // Siempre retorna true en modo mock
            // Si quieres simular un token inválido, puedes cambiar esto
            return Task.FromResult(true);
        }
    }

    /// <summary>
    /// Mock del servicio de bitácoras (SRV18).
    /// TEMPORAL: Solo escribe en consola/log para poder trabajar sin depender del compañero.
    /// 
    /// CUANDO EL COMPAÑERO TERMINE SRV18:
    /// 1. Crear una clase que implemente IBitacoraService
    ///    y haga un HTTP call real al endpoint /bitacora.
    /// 2. En Program.cs cambiar:
    ///    builder.Services.AddScoped<IBitacoraService, MockBitacoraService>();
    ///    POR:
    ///    builder.Services.AddScoped<IBitacoraService, RealBitacoraService>();
    /// </summary>
    public class MockBitacoraService : IBitacoraService
    {
        private readonly ILogger<MockBitacoraService> _logger;

        public MockBitacoraService(ILogger<MockBitacoraService> logger)
        {
            _logger = logger;
        }

        public Task RegistrarBitacoraAsync(string usuario, string descripcion, string token)
        {
            _logger.LogWarning("[MOCK BITÁCORA] Usuario: {Usuario} | Acción: {Descripcion}", 
                usuario, descripcion);
            
            return Task.CompletedTask;
        }
    }
}
