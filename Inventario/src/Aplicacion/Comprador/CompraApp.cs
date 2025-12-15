using System.Text.Json;
using Aplicacion.Modelos;
using Aplicacion.Utils;
using Dominio.Modelos;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Comprador;

public class CompraApp : ICompraApp
{
    private readonly ILogger<CompraApp> _logger;
    private readonly ICompraRepositorio _repo;

    public CompraApp(ILogger<CompraApp> logger, ICompraRepositorio repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<RespuestaDto<CompraResultadoDto>> ProcesarCompraAsync(CompraCrearDto body, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "ProcesarCompra", body);

        if (body.Detalles == null || body.Detalles.Count == 0)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "La compra debe tener al menos un detalle.");
        var json = JsonSerializer.Serialize(new
        {
            id_usuario = body.IdUsuario,
            moneda = body.Moneda ?? "USD",
            detalles = body.Detalles.Select(d => new
            {
                id_producto_proveedor_lote = d.IdProductoProveedorLote,
                cantidad = d.Cantidad
            })
        });

        var sp = await _repo.ProcesarCompraAsync(json, ct);

        if (sp == null)
            throw new CustomException(TipoErrorEnum.ERROR_INTERNO, "No se obtuvo respuesta del proceso de compra.");

        if (!sp.IsSuccess)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, sp.Message);

        var resp = new RespuestaDto<CompraResultadoDto>
        {
            IsSuccess = true,
            Message = sp.Message,
            Data = new CompraResultadoDto
            {
                IdPedido = sp.IdPedido,
                Total = sp.Total
            }
        };

        _logger.LogsEnd("App", "ProcesarCompra", resp);
        return resp;
    }
}
