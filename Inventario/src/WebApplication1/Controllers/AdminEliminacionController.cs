using System.Security.Claims;
using Aplicacion.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Admin - Eliminación Lógica")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "ADMIN")]
    [Route("api/v1/admin")]
    public class AdminEliminacionController : ControllerBase
    {
        private readonly ILogger<AdminEliminacionController> _log;
        private readonly IEliminacionApp _app;

        public AdminEliminacionController(ILogger<AdminEliminacionController> log, IEliminacionApp app)
        {
            _log = log;
            _app = app;
        }

        private string ObtenerUsuarioModificacion()
        {
            var correo = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(correo)) return correo;

            var id = User?.Claims?.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
            return string.IsNullOrWhiteSpace(id) ? "sistema" : $"usuario_{id}";
        }

        [HttpDelete("categorias/{idCategoria:int}")]
        public async Task<IActionResult> DesactivarCategoria([FromRoute] int idCategoria, CancellationToken ct)
        {
            var usuario = ObtenerUsuarioModificacion();

            using (_log.BeginScope("DesactivarCategoria -> {idCategoria} | {usuario}", idCategoria, usuario))
            {
                var resp = await _app.DesactivarCategoriaAsync(idCategoria, usuario, ct);
                if (!resp.IsSuccess) return BadRequest(resp);
                return Ok(resp);
            }
        }

        [HttpDelete("productos/{idProducto:int}")]
        public async Task<IActionResult> DesactivarProducto([FromRoute] int idProducto, CancellationToken ct)
        {
            var usuario = ObtenerUsuarioModificacion();

            using (_log.BeginScope("DesactivarProducto -> {idProducto} | {usuario}", idProducto, usuario))
            {
                var resp = await _app.DesactivarProductoAsync(idProducto, usuario, ct);
                if (!resp.IsSuccess) return BadRequest(resp);
                return Ok(resp);
            }
        }

        [HttpDelete("proveedores/{idProveedor:int}")]
        public async Task<IActionResult> DesactivarProveedor([FromRoute] int idProveedor, CancellationToken ct)
        {
            var usuario = ObtenerUsuarioModificacion();

            using (_log.BeginScope("DesactivarProveedor -> {idProveedor} | {usuario}", idProveedor, usuario))
            {
                var resp = await _app.DesactivarProveedorAsync(idProveedor, usuario, ct);
                if (!resp.IsSuccess) return BadRequest(resp);
                return Ok(resp);
            }
        }

        [HttpDelete("ofertas/{idProductoProveedorLote:int}")]
        public async Task<IActionResult> DesactivarOferta([FromRoute] int idProductoProveedorLote, CancellationToken ct)
        {
            var usuario = ObtenerUsuarioModificacion();

            using (_log.BeginScope("DesactivarOferta -> {idProductoProveedorLote} | {usuario}", idProductoProveedorLote, usuario))
            {
                var resp = await _app.DesactivarOfertaAsync(idProductoProveedorLote, usuario, ct);
                if (!resp.IsSuccess) return BadRequest(resp);
                return Ok(resp);
            }
        }
    }
}
