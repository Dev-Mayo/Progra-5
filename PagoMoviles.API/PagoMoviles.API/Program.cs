using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PagoMoviles.Abstract;
using PagoMoviles.BusinessLogic;
using PagoMoviles.DataAccess.Contexts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// BASES DE DATOS

builder.Services.AddDbContext<CoreBancarioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreBancario"))); // Base de Diego (Cuentas)

builder.Services.AddDbContext<PagosMovilesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PagosMoviles"))); // Base de David (Transacciones)

builder.Services.AddDbContext<AfiliacionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AfiliacionDB"))); // Base de Geancarlo (Teléfonos)


// 2 llamar servicios de compañeros

builder.Services.AddHttpClient();




builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>();


//  JWT

var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
// DESHABILITAR VALIDACIÓN AUTOMÁTICA
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


//  SWAGGER CON TOKEN

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pagos Móviles",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token: Bearer {tu token aquí}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run(); 