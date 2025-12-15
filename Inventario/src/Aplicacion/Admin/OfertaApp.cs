using Aplicacion.Modelos;
using Aplicacion.Utils;
using AutoMapper;
using Dominio.Modelos;
using Dominio.Modelos.Entidades;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Admin;

public class OfertaApp : IOfertaApp
{
    private readonly IMapper _mapper;
    private readonly ILogger<OfertaApp> _logger;
    private readonly IProductoProveedorLoteRepositorio _repo;

    public OfertaApp(IMapper mapper, ILogger<OfertaApp> logger, IProductoProveedorLoteRepositorio repo)
    {
        _mapper = mapper;
        _logger = logger;
        _repo = repo;
    }

    public async Task<List<OfertaDto>> GetOfertasPorProductoAsync(int idProducto, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "GetOfertasPorProducto", new { idProducto });

        var entidades = await _repo.ObtenerPorProductoAsync(idProducto, ct);

        if (entidades == null || entidades.Count == 0)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "No existen ofertas para este producto.");

        var result = _mapper.Map<List<OfertaDto>>(entidades);

        _logger.LogsEnd("App", "GetOfertasPorProducto", result);
        return result;
    }

    public async Task<OfertaDto?> GetOfertaPorIdAsync(int idProductoProveedorLote, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "GetOfertaPorId", new { idProductoProveedorLote });

        var entidad = await _repo.ObtenerPorIdAsync(idProductoProveedorLote, ct);

        if (entidad == null)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "Oferta no encontrada.");

        var dto = _mapper.Map<OfertaDto>(entidad);

        _logger.LogsEnd("App", "GetOfertaPorId", dto);
        return dto;
    }

    public async Task<RespuestaDto<int>> CrearOfertaAsync(OfertaCrearDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "CrearOferta", body);

        var entidad = _mapper.Map<ProductoProveedorLote>(body);
        entidad.UsuarioCreacion = usuario;

        var id = await _repo.InsertarAsync(entidad, ct);

        if (id <= 0)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo crear la oferta.");

        var resp = new RespuestaDto<int>
        {
            IsSuccess = true,
            Message = "Oferta creada correctamente.",
            Data = id
        };

        _logger.LogsEnd("App", "CrearOferta", resp);
        return resp;
    }

    public async Task<RespuestaDto<bool>> ActualizarOfertaAsync(OfertaActualizarDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "ActualizarOferta", body);

        var entidad = _mapper.Map<ProductoProveedorLote>(body);
        entidad.UsuarioModificacion = usuario;

        var ok = await _repo.ActualizarAsync(entidad, ct);

        if (!ok)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo actualizar la oferta.");

        var resp = new RespuestaDto<bool>
        {
            IsSuccess = true,
            Message = "Oferta actualizada correctamente.",
            Data = true
        };

        _logger.LogsEnd("App", "ActualizarOferta", resp);
        return resp;
    }
}
