using Catalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IMongoCollection<Producto> _productos;

        public ProductosController(IMongoCollection<Producto> productos)
        {
            _productos = productos;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var lista = await _productos.Find(p => true).ToListAsync();
            return Ok(lista);
        }

        // GET: api/productos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> Get(string id)
        {
            var producto = await _productos.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<Producto>> Post([FromBody] Producto nuevoProducto)
        {
            await _productos.InsertOneAsync(nuevoProducto);
            return CreatedAtAction(nameof(Get), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        // ACTUALIZAR (Update)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Producto productoIn)
        {
            var producto = await _productos.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (producto == null) return NotFound();

            await _productos.ReplaceOneAsync(p => p.Id == id, productoIn);
            return NoContent();
        }

        // ELIMINAR (Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var producto = await _productos.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (producto == null) return NotFound();

            await _productos.DeleteOneAsync(p => p.Id == id);
            return NoContent();
        }
    }
}
