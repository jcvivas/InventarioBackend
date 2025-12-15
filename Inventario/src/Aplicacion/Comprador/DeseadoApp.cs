using Aplicacion.Modelos;
using Aplicacion.Utils;
using Dominio.Modelos;
using Dominio.Modelos.Entidades;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Comprador;

public class DeseadoApp : IDeseadoApp
{
    private readonly ILogger<DeseadoApp> _logger;
    private readonly IDeseadoRepositorio _repo;

    public DeseadoApp(ILogger<DeseadoApp> logger, IDeseadoRepositorio repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<List<DeseadoDto>> GetDeseadosAsync(int idUsuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "GetDeseados", new { idUsuario });

        var lista = await _repo.ObtenerPorUsuarioAsync(idUsuario, ct);

        if (lista == null || lista.Count == 0)
            return new List<DeseadoDto>();

        var result = lista.Select(x => new DeseadoDto
        {
            IdProducto = x.idProducto,
            FechaAgregadoUtc = x.fechaAgregadoUtc
        }).ToList();

        _logger.LogsEnd("App", "GetDeseados", result);
        return result;
    }

    public async Task<RespuestaDto<bool>> ToggleDeseadoAsync(int idUsuario, int idProducto, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "ToggleDeseado", new { idUsuario, idProducto });

        var ok = await _repo.ToggleAsync(idUsuario, idProducto, ct);

        if (!ok)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo actualizar el producto deseado.");

        var resp = new RespuestaDto<bool>
        {
            IsSuccess = true,
            Message = "Operación realizada correctamente.",
            Data = true
        };

        _logger.LogsEnd("App", "ToggleDeseado", resp);
        return resp;
    }
}
