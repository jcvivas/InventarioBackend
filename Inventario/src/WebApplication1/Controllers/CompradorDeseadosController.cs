using Aplicacion.Comprador;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Comprador - Deseados")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "COMPRADOR")]
    [Route("api/v1/comprador/deseados")]
    public class CompradorDeseadosController : ControllerBase
    {
        private readonly ILogger<CompradorDeseadosController> _log;
        private readonly IDeseadoApp _app;

        public CompradorDeseadosController(ILogger<CompradorDeseadosController> log, IDeseadoApp app)
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

        [HttpGet]
        [ProducesResponseType(typeof(List<DeseadoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario <= 0) return Unauthorized();

            using (_log.BeginScope("Comprador Deseados -> Get | usuario {idUsuario}", idUsuario))
            {
                var data = await _app.GetDeseadosAsync(idUsuario, ct);
                return Ok(data);
            }
        }

        [HttpPost("{idProducto:int}/toggle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Toggle([FromRoute] int idProducto, CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario <= 0) return Unauthorized();

            using (_log.BeginScope("Comprador Deseados -> Toggle {idProducto} | usuario {idUsuario}", idProducto, idUsuario))
            {
                var resp = await _app.ToggleDeseadoAsync(idUsuario, idProducto, ct);
                return Ok(resp);
            }
        }
    }
}
