using Aplicacion.Modelos;

namespace Aplicacion.Admin
{
    public interface IMonitoreoInventarioApp
    {
        Task<RespuestaDto<List<MovimientoInventarioDto>>> ObtenerMovimientosPorOfertaAsync(int idProductoProveedorLote, CancellationToken ct = default);
    }
}
