using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TareaCorta2.Config;
using TareaCorta2.DTO;
using TareaCorta2.Interface;
using TareaCorta2.Model;

namespace TareaCorta2.BL
{
    public class ClienteService : IClienteService
    {
        private readonly IMongoCollection<Cliente> _clientes;

        public ClienteService(IOptions<MongoSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _clientes = database.GetCollection<Cliente>(settings.Value.ClientesCollection);
        }

        public async Task<ApiResponse<Cliente>> CrearAsync(ClienteCreateDto dto)
        {

            var existeIdentificacion = await _clientes
                .Find(c => c.Identificacion == dto.Identificacion)
                .FirstOrDefaultAsync();

            if (existeIdentificacion != null)
            {
                return new ApiResponse<Cliente>
                {
                    Codigo = 409,
                    Descripcion = "La identificación ya está registrada",
                    Data = null
                };
            }

            var existeTelefono = await _clientes
                .Find(c => c.Telefono == dto.Telefono)
                .FirstOrDefaultAsync();

            if (existeTelefono != null)
            {
                return new ApiResponse<Cliente>
                {
                    Codigo = 409,
                    Descripcion = "El teléfono ya está registrado",
                    Data = null
                };
            }

            var cliente = new Cliente
            {
                NombreCompleto = dto.NombreCompleto.Trim(),
                TipoIdentificacion = dto.TipoIdentificacion,
                Identificacion = dto.Identificacion,
                Telefono = dto.Telefono,
                Email = dto.Email
            };

            await _clientes.InsertOneAsync(cliente);

            return new ApiResponse<Cliente>
            {
                Codigo = 201,
                Descripcion = "Cliente creado correctamente",
                Data = cliente
            };
        }

        public async Task<ApiResponse<List<Cliente>>> ObtenerTodosAsync()
        {
            var clientes = await _clientes.Find(_ => true).ToListAsync();

            return new ApiResponse<List<Cliente>>
            {
                Codigo = 200,
                Descripcion = "Lista de clientes",
                Data = clientes
            };
        }

        public async Task<ApiResponse<Cliente>> ObtenerPorIdAsync(string id)
        {
            var cliente = await _clientes.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (cliente == null)
            {
                return new ApiResponse<Cliente>
                {
                    Codigo = 404,
                    Descripcion = "Cliente no encontrado",
                    Data = null
                };
            }

            return new ApiResponse<Cliente>
            {
                Codigo = 200,
                Descripcion = "Cliente encontrado",
                Data = cliente
            };
        }
    }
}
