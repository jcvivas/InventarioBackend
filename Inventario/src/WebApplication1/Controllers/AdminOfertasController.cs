using Aplicacion.Admin;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Admin - Ofertas")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "ADMIN")]
    [Route("api/v1/admin/ofertas")]
    public class AdminOfertasController : ControllerBase
    {
        private readonly ILogger<AdminOfertasController> _log;
        private readonly IOfertaApp _app;

        public AdminOfertasController(ILogger<AdminOfertasController> log, IOfertaApp app)
        {
            _log = log;
            _app = app;
        }

        [HttpGet("producto/{idProducto:int}")]
        [ProducesResponseType(typeof(List<OfertaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPorProducto([FromRoute] int idProducto, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Ofertas -> GetPorProducto {idProducto}", idProducto))
            {
                var data = await _app.GetOfertasPorProductoAsync(idProducto, ct);
                return Ok(data);
            }
        }

        [HttpGet("{idProductoProveedorLote:int}")]
        [ProducesResponseType(typeof(OfertaDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int idProductoProveedorLote, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Ofertas -> GetById {id}", idProductoProveedorLote))
            {
                var data = await _app.GetOfertaPorIdAsync(idProductoProveedorLote, ct);
                return Ok(data);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfertaCrearDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Ofertas -> Create {body}", body))
            {
                var usuario = User?.Identity?.Name;
                var resp = await _app.CrearOfertaAsync(body, usuario, ct);
                return Ok(resp);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] OfertaActualizarDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Ofertas -> Update {body}", body))
            {
                var usuario = User?.Identity?.Name;
                var resp = await _app.ActualizarOfertaAsync(body, usuario, ct);
                return Ok(resp);
            }
        }
    }
}
