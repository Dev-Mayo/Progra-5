using Microsoft.EntityFrameworkCore;
using PagoMoviles.Data;
using PagoMoviles.Services.Implementations;
using PagoMoviles.Services.Interfaces;
using PagoMoviles.Services.Mocks;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════════════════
// CONFIGURACIÓN DE ENTITY FRAMEWORK - DOS BASES DE DATOS
// ═══════════════════════════════════════════════════════════

// BD de Pagos Móviles (nuestra tabla monedero_movil)
builder.Services.AddDbContext<PagosMovilesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// BD del Core Bancario (tablas cliente, cuenta, movimiento - creadas por otro compañero)
builder.Services.AddDbContext<CoreBancarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreBancarioDB")));

// ═══════════════════════════════════════════════════════════
// INYECCIÓN DE DEPENDENCIAS - NUESTROS SERVICIOS
// ═══════════════════════════════════════════════════════════

// SRV19 - Verificar si un cliente existe en el core
builder.Services.AddScoped<IClienteService, ClienteService>();

// SRV9 y SRV10 - Inscripción y desinscripción de pagos móviles
builder.Services.AddScoped<IPagoMovilService, PagoMovilService>();

// ═══════════════════════════════════════════════════════════
// MOCKS - SERVICIOS DE OTROS COMPAÑEROS (TEMPORALES)
// ═══════════════════════════════════════════════════════════
// 
// IMPORTANTE: Cuando los compañeros terminen sus servicios,
// cambiar las líneas de abajo por las implementaciones reales.
//
// Ejemplo para SRV5 (login/validate):
//   builder.Services.AddScoped<ITokenValidationService, RealTokenValidationService>();
//
// Ejemplo para SRV18 (bitácoras):
//   builder.Services.AddScoped<IBitacoraService, RealBitacoraService>();

builder.Services.AddScoped<ITokenValidationService, MockTokenValidationService>();
builder.Services.AddScoped<IBitacoraService, MockBitacoraService>();

// ═══════════════════════════════════════════════════════════
// CONFIGURACIÓN GENERAL
// ═══════════════════════════════════════════════════════════

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Pagos Móviles API",
        Version = "v1",
        Description = "API de Pagos Móviles - SRV9, SRV10, SRV19"
    });

    // Configurar el botón "Authorize" en Swagger para enviar el token
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Escribí: Bearer token123"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS (para que otros grupos/servicios puedan consumir la API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
