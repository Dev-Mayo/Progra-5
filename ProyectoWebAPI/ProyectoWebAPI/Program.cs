using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.DataAccess;
using ProyectoWebAPI.BusinessLogic;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess.Models;
using ProyectoWebAPI.Validation;

namespace ProyectoWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();


            builder.Services.AddDbContext<ClienteContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<PantallaContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<RolesContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<EntidadesBancariasContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddHttpClient<IBitacoraClient, BitacoraClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7232");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
            });

            builder.Services.AddHttpClient<ITokenService, TokenService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7160");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
            });


            builder.Services.AddScoped<ClienteRepository>();
            builder.Services.AddScoped<PantallaRepository>();
            builder.Services.AddScoped<RolesRepository>();
            builder.Services.AddScoped<EntidadesBancariasRepository>();


            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IPantallaService, PantallaService>();
            builder.Services.AddScoped<IRolService, RolService>();
            builder.Services.AddScoped<IEntidadesService, EntidadesService>();


            builder.Services.AddScoped<ITokenService, TokenService>();

  
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<TokenValidation>();

            app.MapControllers();
            app.Run();
        }
    }
}