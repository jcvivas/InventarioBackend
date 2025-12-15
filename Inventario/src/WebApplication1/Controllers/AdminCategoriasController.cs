using Aplicacion.Admin;
using Aplicacion.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Admin - Categorías")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "ADMIN")]
    [Route("api/v1/admin/categorias")]
    public class AdminCategoriasController : ControllerBase
    {
        private readonly ILogger<AdminCategoriasController> _log;
        private readonly ICategoriaApp _app;

        public AdminCategoriasController(ILogger<AdminCategoriasController> log, ICategoriaApp app)
        {
            _log = log;
            _app = app;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            using (_log.BeginScope("Admin Categorías -> GetAll"))
            {
                var data = await _app.GetCategoriasAsync(ct);
                return Ok(data);
            }
        }

        [HttpGet("{idCategoria:int}")]
        [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int idCategoria, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Categorías -> GetById {idCategoria}", idCategoria))
            {
                var data = await _app.GetCategoriaPorIdAsync(idCategoria, ct);
                return Ok(data);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CategoriaCrearDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Categorías -> Create {body}", body))
            {
                var usuario = User?.Identity?.Name; 
                var resp = await _app.CrearCategoriaAsync(body, usuario, ct);
                return Ok(resp);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CategoriaActualizarDto body, CancellationToken ct)
        {
            using (_log.BeginScope("Admin Categorías -> Update {body}", body))
            {
                var usuario = User?.Identity?.Name;
                var resp = await _app.ActualizarCategoriaAsync(body, usuario, ct);
                return Ok(resp);
            }
        }
    }
}
