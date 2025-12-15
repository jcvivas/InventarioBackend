using Aplicacion.Modelos;
using Aplicacion.Utils;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Admin
{
    public class MonitoreoInventarioApp : IMonitoreoInventarioApp
    {
        private readonly ILogger<MonitoreoInventarioApp> _logger;
        private readonly IMovimientoInventarioRepositorio _repoMov;

        public MonitoreoInventarioApp(
            ILogger<MonitoreoInventarioApp> logger,
            IMovimientoInventarioRepositorio repoMov)
        {
            _logger = logger;
            _repoMov = repoMov;
        }

        public async Task<RespuestaDto<List<MovimientoInventarioDto>>> ObtenerMovimientosPorOfertaAsync(int idProductoProveedorLote, CancellationToken ct = default)
        {
            _logger.LogsInit("App", "ObtenerMovimientosPorOferta", new { idProductoProveedorLote });

            var lista = await _repoMov.ObtenerPorProductoProveedorLoteAsync(idProductoProveedorLote, ct);

            var dto = (lista ?? new List<Dominio.Modelos.Entidades.MovimientoInventario>())
                .Select(x => new MovimientoInventarioDto
                {
                    IdMovimiento = x.IdMovimiento,
                    IdProductoProveedorLote = x.IdProductoProveedorLote,
                    TipoMovimiento = x.TipoMovimiento,
                    Cantidad = x.Cantidad,
                    Motivo = x.Motivo,
                    Referencia = x.Referencia,
                    IdUsuario = x.IdUsuario,
                    FechaMovimientoUtc = x.FechaMovimientoUtc
                })
                .ToList();

            var resp = new RespuestaDto<List<MovimientoInventarioDto>>
            {
                IsSuccess = true,
                Message = "Consulta exitosa.",
                Data = dto
            };

            _logger.LogsEnd("App", "ObtenerMovimientosPorOferta", new { total = dto.Count });
            return resp;
        }
    }
}
