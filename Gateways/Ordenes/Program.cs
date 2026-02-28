using MongoDB.Driver;
using Ordenes.Models;

namespace Ordenes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configurar el Cliente de MongoDB para Órdenes
            var mongoClient = new MongoClient(builder.Configuration.GetSection("MongoSettings:ConnectionString").Value);
            var database = mongoClient.GetDatabase(builder.Configuration.GetSection("MongoSettings:DatabaseName").Value);

            // 2. REGISTRAR LA COLECCIÓN (Esto es lo que falta y causa el error)
            builder.Services.AddSingleton(database.GetCollection<Pedido>("Pedidos"));

            // 3. Registrar HttpClient para la comunicación con Catálogo
            builder.Services.AddHttpClient("CatalogoService", client =>
            {
                // Revisa que este puerto sea el de tu Catalogo.API
                client.BaseAddress = new Uri("https://localhost:7273/");
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
