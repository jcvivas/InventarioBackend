using Aplicacion.Modelos;
using Aplicacion.Utils;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Admin
{
    public class EliminacionApp : IEliminacionApp
    {
        private readonly ILogger<EliminacionApp> _logger;
        private readonly ICategoriaRepositorio _categoriaRepo;
        private readonly IProductoRepositorio _productoRepo;
        private readonly IProveedorRepositorio _proveedorRepo;
        private readonly IProductoProveedorLoteRepositorio _ofertaRepo;

        public EliminacionApp(
            ILogger<EliminacionApp> logger,
            ICategoriaRepositorio categoriaRepo,
            IProductoRepositorio productoRepo,
            IProveedorRepositorio proveedorRepo,
            IProductoProveedorLoteRepositorio ofertaRepo)
        {
            _logger = logger;
            _categoriaRepo = categoriaRepo;
            _productoRepo = productoRepo;
            _proveedorRepo = proveedorRepo;
            _ofertaRepo = ofertaRepo;
        }

        public async Task<RespuestaDto<bool>> DesactivarCategoriaAsync(int idCategoria, string? usuarioModificacion, CancellationToken ct = default)
        {
            _logger.LogsInit("App", "DesactivarCategoria", new { idCategoria, usuarioModificacion });

            var entidad = await _categoriaRepo.ObtenerPorIdAsync(idCategoria, ct);
            if (entidad is null)
            {
                _logger.LogsEnd("App", "DesactivarCategoria", "No existe categoría");
                return new RespuestaDto<bool> { IsSuccess = false, Message = "No existe la categoría.", Data = false };
            }

            entidad.Activo = false;

            var ok = await _categoriaRepo.ActualizarAsync(entidad, ct);

            _logger.LogsEnd("App", "DesactivarCategoria", new { ok });
            return new RespuestaDto<bool>
            {
                IsSuccess = ok,
                Message = ok ? "Categoría desactivada." : "No se pudo desactivar la categoría.",
                Data = ok
            };
        }

        public async Task<RespuestaDto<bool>> DesactivarProductoAsync(int idProducto, string? usuarioModificacion, CancellationToken ct = default)
        {
            _logger.LogsInit("App", "DesactivarProducto", new { idProducto, usuarioModificacion });

            var entidad = await _productoRepo.ObtenerPorIdAsync(idProducto, ct);
            if (entidad is null)
            {
                _logger.LogsEnd("App", "DesactivarProducto", "No existe producto");
                return new RespuestaDto<bool> { IsSuccess = false, Message = "No existe el producto.", Data = false };
            }

            entidad.Activo = false;
            entidad.UsuarioModificacion = usuarioModificacion;

            var ok = await _productoRepo.ActualizarAsync(entidad, ct);

            _logger.LogsEnd("App", "DesactivarProducto", new { ok });
            return new RespuestaDto<bool>
            {
                IsSuccess = ok,
                Message = ok ? "Producto desactivado." : "No se pudo desactivar el producto.",
                Data = ok
            };
        }

        public async Task<RespuestaDto<bool>> DesactivarProveedorAsync(int idProveedor, string? usuarioModificacion, CancellationToken ct = default)
        {
            _logger.LogsInit("App", "DesactivarProveedor", new { idProveedor, usuarioModificacion });

            var entidad = await _proveedorRepo.ObtenerPorIdAsync(idProveedor, ct);
            if (entidad is null)
            {
                _logger.LogsEnd("App", "DesactivarProveedor", "No existe proveedor");
                return new RespuestaDto<bool> { IsSuccess = false, Message = "No existe el proveedor.", Data = false };
            }

            entidad.Activo = false;
            entidad.UsuarioModificacion = usuarioModificacion;

            var ok = await _proveedorRepo.ActualizarAsync(entidad, ct);

            _logger.LogsEnd("App", "DesactivarProveedor", new { ok });
            return new RespuestaDto<bool>
            {
                IsSuccess = ok,
                Message = ok ? "Proveedor desactivado." : "No se pudo desactivar el proveedor.",
                Data = ok
            };
        }

        public async Task<RespuestaDto<bool>> DesactivarOfertaAsync(int idProductoProveedorLote, string? usuarioModificacion, CancellationToken ct = default)
        {
            _logger.LogsInit("App", "DesactivarOferta", new { idProductoProveedorLote, usuarioModificacion });

            var entidad = await _ofertaRepo.ObtenerPorIdAsync(idProductoProveedorLote, ct);
            if (entidad is null)
            {
                _logger.LogsEnd("App", "DesactivarOferta", "No existe oferta");
                return new RespuestaDto<bool> { IsSuccess = false, Message = "No existe la oferta.", Data = false };
            }

            entidad.Activo = false;
            entidad.UsuarioModificacion = usuarioModificacion;

            var ok = await _ofertaRepo.ActualizarAsync(entidad, ct);
            _logger.LogsEnd("App", "DesactivarOferta", new { ok });
            return new RespuestaDto<bool>
            {
                IsSuccess = ok,
                Message = ok ? "Oferta desactivada." : "No se pudo desactivar la oferta.",
                Data = ok
            };
        }
    }
}
