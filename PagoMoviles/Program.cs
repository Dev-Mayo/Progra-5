using Microsoft.EntityFrameworkCore;
using PagoMoviles.Data;
using PagoMoviles.Services.Implementations;
using PagoMoviles.Services.Interfaces;
using PagoMoviles.Services.Mocks;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<PagosMovilesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDbContext<CoreBancarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreBancarioDB")));



// SRV19 - Verificar si un cliente existe en el core
builder.Services.AddScoped<IClienteService, ClienteService>();

// SRV9 y SRV10 - Inscripción y desinscripción de pagos móviles
builder.Services.AddScoped<IPagoMovilService, PagoMovilService>();



builder.Services.AddHttpClient<ITokenValidationService, RealTokenValidationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7160");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
});
builder.Services.AddHttpClient<IBitacoraService, RealBitacoraService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7232");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
});



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
