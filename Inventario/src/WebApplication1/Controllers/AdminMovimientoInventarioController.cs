using Aplicacion.Admin;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Admin - Monitoreo Inventario")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "ADMIN")]
    [Route("api/v1/admin/inventario")]
    public class AdminMonitoreoInventarioController : ControllerBase
    {
        private readonly ILogger<AdminMonitoreoInventarioController> _log;
        private readonly IMonitoreoInventarioApp _app;

        public AdminMonitoreoInventarioController(
            ILogger<AdminMonitoreoInventarioController> log,
            IMonitoreoInventarioApp app)
        {
            _log = log;
            _app = app;
        }

        [HttpGet("movimientos/{idProductoProveedorLote:int}")]
        [ProducesResponseType(typeof(List<MovimientoInventarioDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MovimientosPorOferta([FromRoute] int idProductoProveedorLote, CancellationToken ct)
        {
            using (_log.BeginScope("MovimientosPorOferta -> {idProductoProveedorLote}", idProductoProveedorLote))
            {
                var resp = await _app.ObtenerMovimientosPorOfertaAsync(idProductoProveedorLote, ct);
                return Ok(resp);
            }
        }
    }
}
