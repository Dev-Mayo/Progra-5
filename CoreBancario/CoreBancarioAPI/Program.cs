using CoreBancarioAPI;
using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic;
using CoreBancarioService.DataAccess;
using CoreBancarioService.DataAccess.Models;
using CoreBancarioService.DataAccess.Repositories;
using CoreBancarioService.Abstract.Security;
using CoreBancarioService.BusinessLogic.Security;
using CoreBancarioAPI.Validation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CoreBancarioContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddHttpClient<ITokenValidationService, TokenValidationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7160");
});

builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<IUltimoMovimientoService, UltimoMovimientoService>();
builder.Services.AddScoped<IUltimoMovimientoRepository, UltimoMovimientoRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TokenValidation>();

app.MapCuentaEndpoints();
app.MapMovimientoEndpoints();

app.Run();
