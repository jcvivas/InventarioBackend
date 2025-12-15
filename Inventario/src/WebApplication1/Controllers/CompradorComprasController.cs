using Aplicacion.Comprador;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Comprador - Compras")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "COMPRADOR")]
    [Route("api/v1/comprador/compras")]
    public class CompradorComprasController : ControllerBase
    {
        private readonly ILogger<CompradorComprasController> _log;
        private readonly ICompraApp _app;

        public CompradorComprasController(ILogger<CompradorComprasController> log, ICompraApp app)
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

        public class CompraRequestDto
        {
            public string Moneda { get; set; } = "USD";
            public List<CompraDetalleDto> Detalles { get; set; } = new();
        }

        [HttpPost("procesar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Procesar([FromBody] CompraRequestDto body, CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario <= 0) return Unauthorized();

            using (_log.BeginScope("Comprador Compras -> Procesar | usuario {idUsuario} | body {body}", idUsuario, body))
            {
                var dto = new CompraCrearDto
                {
                    IdUsuario = idUsuario,
                    Moneda = body.Moneda ?? "USD",
                    Detalles = body.Detalles
                };

                var resp = await _app.ProcesarCompraAsync(dto, ct);
                return Ok(resp);
            }
        }
    }
}
