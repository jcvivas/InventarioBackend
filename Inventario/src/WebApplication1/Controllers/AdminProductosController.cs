using Aplicacion.Admin;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Admin - Productos")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "ADMIN")]
    [Route("api/v1/admin/productos")]
    public class AdminProductosController : ControllerBase
    {
        private readonly ILogger<AdminProductosController> _log;
        private readonly IProductoApp _app;

        public AdminProductosController(ILogger<AdminProductosController> log, IProductoApp app)
        {
            _log = log;
            _app = app;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Buscar([FromQuery] string? texto, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Productos -> Buscar {texto}", texto))
            {
                var data = await _app.BuscarProductosAsync(texto, ct);
                return Ok(data);
            }
        }

        [HttpGet("{idProducto:int}")]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int idProducto, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Productos -> GetById {idProducto}", idProducto))
            {
                var data = await _app.GetProductoPorIdAsync(idProducto, ct);
                return Ok(data);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ProductoCrearDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Productos -> Create {body}", body))
            {
                var usuario = User?.Identity?.Name;
                var resp = await _app.CrearProductoAsync(body, usuario, ct);
                return Ok(resp);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ProductoActualizarDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Productos -> Update {body}", body))
            {
                var usuario = User?.Identity?.Name;
                var resp = await _app.ActualizarProductoAsync(body, usuario, ct);
                return Ok(resp);
            }
        }
    }
}
