using Catalogo.Models;
using MongoDB.Driver;

namespace Catalogo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Configuración de MongoDB
            var mongoClient = new MongoClient(builder.Configuration.GetSection("MongoSettings:ConnectionString").Value);
            var database = mongoClient.GetDatabase(builder.Configuration.GetSection("MongoSettings:DatabaseName").Value);

            // Inyectamos la colección de productos para que el controlador la use
            builder.Services.AddSingleton(database.GetCollection<Producto>("catalogo"));

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
