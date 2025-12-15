using Aplicacion.Modelos;
using Aplicacion.Utils;
using AutoMapper;
using Dominio.Modelos;
using Dominio.Modelos.Entidades;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Admin;

public class ProveedorApp : IProveedorApp
{
    private readonly IMapper _mapper;
    private readonly ILogger<ProveedorApp> _logger;
    private readonly IProveedorRepositorio _repo;

    public ProveedorApp(IMapper mapper, ILogger<ProveedorApp> logger, IProveedorRepositorio repo)
    {
        _mapper = mapper;
        _logger = logger;
        _repo = repo;
    }

    public async Task<List<ProveedorDto>> BuscarProveedoresAsync(string? texto, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "BuscarProveedores", new { texto });

        var entidades = await _repo.BuscarAsync(texto, ct);
        var result = _mapper.Map<List<ProveedorDto>>(entidades);

        _logger.LogsEnd("App", "BuscarProveedores", result);
        return result;
    }

    public async Task<ProveedorDto?> GetProveedorPorIdAsync(int idProveedor, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "GetProveedorPorId", new { idProveedor });

        var entidad = await _repo.ObtenerPorIdAsync(idProveedor, ct);

        if (entidad == null)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "Proveedor no encontrado.");

        var dto = _mapper.Map<ProveedorDto>(entidad);

        _logger.LogsEnd("App", "GetProveedorPorId", dto);
        return dto;
    }

    public async Task<RespuestaDto<int>> CrearProveedorAsync(ProveedorCrearDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "CrearProveedor", body);

        var entidad = _mapper.Map<Proveedor>(body);
        entidad.UsuarioCreacion = usuario;

        var id = await _repo.InsertarAsync(entidad, ct);

        if (id <= 0)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo crear el proveedor.");

        var resp = new RespuestaDto<int>
        {
            IsSuccess = true,
            Message = "Proveedor creado correctamente.",
            Data = id
        };

        _logger.LogsEnd("App", "CrearProveedor", resp);
        return resp;
    }

    public async Task<RespuestaDto<bool>> ActualizarProveedorAsync(ProveedorActualizarDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "ActualizarProveedor", body);

        var entidad = _mapper.Map<Proveedor>(body);
        entidad.UsuarioModificacion = usuario;

        var ok = await _repo.ActualizarAsync(entidad, ct);

        if (!ok)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo actualizar el proveedor.");

        var resp = new RespuestaDto<bool>
        {
            IsSuccess = true,
            Message = "Proveedor actualizado correctamente.",
            Data = true
        };

        _logger.LogsEnd("App", "ActualizarProveedor", resp);
        return resp;
    }
}
