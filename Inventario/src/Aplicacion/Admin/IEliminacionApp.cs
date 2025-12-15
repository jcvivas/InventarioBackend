using Aplicacion.Modelos;

namespace Aplicacion.Admin
{
    public interface IEliminacionApp
    {
        Task<RespuestaDto<bool>> DesactivarCategoriaAsync(int idCategoria, string? usuarioModificacion, CancellationToken ct = default);
        Task<RespuestaDto<bool>> DesactivarProductoAsync(int idProducto, string? usuarioModificacion, CancellationToken ct = default);
        Task<RespuestaDto<bool>> DesactivarProveedorAsync(int idProveedor, string? usuarioModificacion, CancellationToken ct = default);
        Task<RespuestaDto<bool>> DesactivarOfertaAsync(int idProductoProveedorLote, string? usuarioModificacion, CancellationToken ct = default);
    }
}
