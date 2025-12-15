using Aplicacion.Admin;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Admin - Proveedores")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "ADMIN")]
    [Route("api/v1/admin/proveedores")]
    public class AdminProveedoresController : ControllerBase
    {
        private readonly ILogger<AdminProveedoresController> _log;
        private readonly IProveedorApp _app;

        public AdminProveedoresController(ILogger<AdminProveedoresController> log, IProveedorApp app)
        {
            _log = log;
            _app = app;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProveedorDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Buscar([FromQuery] string? texto, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Proveedores -> Buscar {texto}", texto))
            {
                var data = await _app.BuscarProveedoresAsync(texto, ct);
                return Ok(data);
            }
        }

        [HttpGet("{idProveedor:int}")]
        [ProducesResponseType(typeof(ProveedorDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int idProveedor, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Proveedores -> GetById {idProveedor}", idProveedor))
            {
                var data = await _app.GetProveedorPorIdAsync(idProveedor, ct);
                return Ok(data);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ProveedorCrearDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Proveedores -> Create {body}", body))
            {
                var usuario = User?.Identity?.Name;
                var resp = await _app.CrearProveedorAsync(body, usuario, ct);
                return Ok(resp);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ProveedorActualizarDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Proveedores -> Update {body}", body))
            {
                var usuario = User?.Identity?.Name;
                var resp = await _app.ActualizarProveedorAsync(body, usuario, ct);
                return Ok(resp);
            }
        }
    }
}
