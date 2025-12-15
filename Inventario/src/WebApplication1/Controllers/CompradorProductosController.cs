using Aplicacion.Comprador;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Comprador - Productos")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "COMPRADOR")]
    [Route("api/v1/comprador/productos")]
    public class CompradorProductosController : ControllerBase
    {
        private readonly ILogger<CompradorProductosController> _log;
        private readonly IProductoCompradorApp _app;

        public CompradorProductosController(ILogger<CompradorProductosController> log, IProductoCompradorApp app)
        {
            _log = log;
            _app = app;
        }

        private int ObtenerIdUsuario()
        {
            var claim = User?.Claims?.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
            if (string.IsNullOrWhiteSpace(claim))
                return 0;

            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet("buscar")]
        [ProducesResponseType(typeof(List<ProductoBusquedaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Buscar([FromQuery] string texto, CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario <= 0) return Unauthorized();

            using (_log.BeginScope("Comprador Productos -> Buscar {texto} | usuario {idUsuario}", texto, idUsuario))
            {
                var data = await _app.BuscarAsync(texto, idUsuario, ct);
                return Ok(data);
            }
        }

        [HttpGet("{idProducto:int}")]
        [ProducesResponseType(typeof(ProductoDetalleDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detalle([FromRoute] int idProducto, CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario <= 0) return Unauthorized();

            using (_log.BeginScope("Comprador Productos -> Detalle {idProducto} | usuario {idUsuario}", idProducto, idUsuario))
            {
                var data = await _app.ObtenerDetalleAsync(idProducto, idUsuario, ct);
                return Ok(data);
            }
        }
    }
}
