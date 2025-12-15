using Aplicacion.Modelos;
using Aplicacion.Utils;
using Dominio.Modelos;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Comprador;

public class ProductoCompradorApp : IProductoCompradorApp
{
    private readonly ILogger<ProductoCompradorApp> _logger;
    private readonly IProductoRepositorio _productoRepo;
    private readonly IProductoProveedorLoteRepositorio _ofertaRepo;
    private readonly ICategoriaRepositorio _categoriaRepo;
    private readonly IProveedorRepositorio _proveedorRepo;
    private readonly IDeseadoRepositorio _deseadoRepo;

    public ProductoCompradorApp(
        ILogger<ProductoCompradorApp> logger,
        IProductoRepositorio productoRepo,
        IProductoProveedorLoteRepositorio ofertaRepo,
        ICategoriaRepositorio categoriaRepo,
        IProveedorRepositorio proveedorRepo,
        IDeseadoRepositorio deseadoRepo)
    {
        _logger = logger;
        _productoRepo = productoRepo;
        _ofertaRepo = ofertaRepo;
        _categoriaRepo = categoriaRepo;
        _proveedorRepo = proveedorRepo;
        _deseadoRepo = deseadoRepo;
    }

    public async Task<List<ProductoBusquedaDto>> BuscarAsync(string texto, int? idUsuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "BuscarProductosComprador", new { texto, idUsuario });

        var productos = await _productoRepo.BuscarAsync(texto, ct);

        if (productos == null || productos.Count == 0)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "No se encontraron productos.");

        var categorias = await _categoriaRepo.ObtenerTodasAsync(ct);

        var result = new List<ProductoBusquedaDto>();

        foreach (var p in productos)
        {
            var ofertas = await _ofertaRepo.ObtenerPorProductoAsync(p.IdProducto, ct);

            var precioDesde = ofertas.Count > 0 ? ofertas.Min(x => x.PrecioUnitario) : 0;
            var stockTotal = ofertas.Count > 0 ? ofertas.Sum(x => Math.Max(0, x.StockDisponible - x.StockReservado)) : 0;

            var esDeseado = false;
            if (idUsuario.HasValue)
                esDeseado = await _deseadoRepo.ExisteAsync(idUsuario.Value, p.IdProducto, ct);

            var catNombre = categorias?.FirstOrDefault(c => c.IdCategoria == p.IdCategoria)?.Nombre;

            result.Add(new ProductoBusquedaDto
            {
                IdProducto = p.IdProducto,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Marca = p.Marca,
                IdCategoria = p.IdCategoria,
                Categoria = catNombre,
                UrlImagen = p.UrlImagen,
                Activo = p.Activo,

                PrecioDesde = precioDesde,
                StockTotalParaVenta = stockTotal,
                Moneda = "USD",

                EsDeseado = esDeseado
            });
        }

        _logger.LogsEnd("App", "BuscarProductosComprador", result);
        return result;
    }

    public async Task<ProductoDetalleDto?> ObtenerDetalleAsync(int idProducto, int? idUsuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "ObtenerDetalleProducto", new { idProducto, idUsuario });

        var producto = await _productoRepo.ObtenerPorIdAsync(idProducto, ct);

        if (producto == null)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "Producto no encontrado.");

        var categoria = producto.IdCategoria.HasValue
            ? await _categoriaRepo.ObtenerPorIdAsync(producto.IdCategoria.Value, ct)
            : null;

        var ofertas = await _ofertaRepo.ObtenerPorProductoAsync(idProducto, ct);

        var esDeseado = false;
        if (idUsuario.HasValue)
            esDeseado = await _deseadoRepo.ExisteAsync(idUsuario.Value, idProducto, ct);

        var detalle = new ProductoDetalleDto
        {
            IdProducto = producto.IdProducto,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Marca = producto.Marca,
            IdCategoria = producto.IdCategoria,
            Categoria = categoria?.Nombre,
            UrlImagen = producto.UrlImagen,
            Activo = producto.Activo,
            EsDeseado = esDeseado
        };

        foreach (var o in ofertas)
        {
            var proveedor = await _proveedorRepo.ObtenerPorIdAsync(o.IdProveedor, ct);

            detalle.Ofertas.Add(new OfertaDetalleDto
            {
                IdProductoProveedorLote = o.IdProductoProveedorLote,
                IdProveedor = o.IdProveedor,
                CodigoProveedor = proveedor?.Codigo,
                NombreProveedor = proveedor?.Nombre,

                NumeroLote = o.NumeroLote,
                PrecioUnitario = o.PrecioUnitario,
                StockParaVenta = Math.Max(0, o.StockDisponible - o.StockReservado),
                Moneda = o.Moneda,
                FechaVencimiento = o.FechaVencimiento,
                OfertaActiva = o.Activo
            });
        }

        _logger.LogsEnd("App", "ObtenerDetalleProducto", detalle);
        return detalle;
    }
}
