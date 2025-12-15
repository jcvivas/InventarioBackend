using Aplicacion.Modelos;
using Aplicacion.Utils;
using AutoMapper;
using Dominio.Modelos;
using Dominio.Modelos.Entidades;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Admin;

public class CategoriaApp : ICategoriaApp
{
    private readonly IMapper _mapper;
    private readonly ILogger<CategoriaApp> _logger;
    private readonly ICategoriaRepositorio _repo;

    public CategoriaApp(IMapper mapper, ILogger<CategoriaApp> logger, ICategoriaRepositorio repo)
    {
        _mapper = mapper;
        _logger = logger;
        _repo = repo;
    }

    public async Task<List<CategoriaDto>> GetCategoriasAsync(CancellationToken ct = default)
    {
        _logger.LogsInit("App", "GetCategorias");

        var entidades = await _repo.ObtenerTodasAsync(ct);

        if (entidades == null || entidades.Count == 0)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "No existen categorías.");

        var result = _mapper.Map<List<CategoriaDto>>(entidades);

        _logger.LogsEnd("App", "GetCategorias", result);
        return result;
    }

    public async Task<CategoriaDto?> GetCategoriaPorIdAsync(int idCategoria, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "GetCategoriaPorId", new { idCategoria });

        var entidad = await _repo.ObtenerPorIdAsync(idCategoria, ct);

        if (entidad == null)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "Categoría no encontrada.");

        var dto = _mapper.Map<CategoriaDto>(entidad);

        _logger.LogsEnd("App", "GetCategoriaPorId", dto);
        return dto;
    }

    public async Task<RespuestaDto<int>> CrearCategoriaAsync(CategoriaCrearDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "CrearCategoria", body);

        var entidad = _mapper.Map<Categoria>(body);

        var id = await _repo.InsertarAsync(entidad, ct);

        if (id <= 0)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo crear la categoría.");

        var resp = new RespuestaDto<int>
        {
            IsSuccess = true,
            Message = "Categoría creada correctamente.",
            Data = id
        };

        _logger.LogsEnd("App", "CrearCategoria", resp);
        return resp;
    }

    public async Task<RespuestaDto<bool>> ActualizarCategoriaAsync(CategoriaActualizarDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "ActualizarCategoria", body);

        var entidad = _mapper.Map<Categoria>(body);

        var ok = await _repo.ActualizarAsync(entidad, ct);

        if (!ok)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo actualizar la categoría.");

        var resp = new RespuestaDto<bool>
        {
            IsSuccess = true,
            Message = "Categoría actualizada correctamente.",
            Data = true
        };

        _logger.LogsEnd("App", "ActualizarCategoria", resp);
        return resp;
    }
}
