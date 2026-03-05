using Microsoft.AspNetCore.Mvc;
using TareaCorta2.DTO;
using TareaCorta2.Interface;

namespace TareaCorta2.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ClienteCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Codigo = -1,
                    Descripcion = "Datos inválidos",
                    Data = null
                });
            }

            var response = await _service.CrearAsync(dto);

            if (response.Codigo == -1)
                return BadRequest(response);

            return Created("", response);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _service.ObtenerTodosAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(string id)
        {
            var response = await _service.ObtenerPorIdAsync(id);

            if (response.Codigo == -1)
                return NotFound(response);

            return Ok(response);
        }
    }
}
