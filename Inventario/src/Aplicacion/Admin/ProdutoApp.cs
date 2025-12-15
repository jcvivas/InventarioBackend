using Aplicacion.Modelos;
using Aplicacion.Utils;
using AutoMapper;
using Dominio.Modelos;
using Dominio.Modelos.Entidades;
using Dominio.Repositorios;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Admin;

public class ProductoApp : IProductoApp
{
    private readonly IMapper _mapper;
    private readonly ILogger<ProductoApp> _logger;
    private readonly IProductoRepositorio _repo;

    public ProductoApp(IMapper mapper, ILogger<ProductoApp> logger, IProductoRepositorio repo)
    {
        _mapper = mapper;
        _logger = logger;
        _repo = repo;
    }

    public async Task<List<ProductoDto>> BuscarProductosAsync(string? texto, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "BuscarProductos", new { texto });

        var entidades = await _repo.BuscarAsync(texto, ct);

        var result = _mapper.Map<List<ProductoDto>>(entidades);

        _logger.LogsEnd("App", "BuscarProductos", result);
        return result;
    }

    public async Task<ProductoDto?> GetProductoPorIdAsync(int idProducto, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "GetProductoPorId", new { idProducto });

        var entidad = await _repo.ObtenerPorIdAsync(idProducto, ct);

        if (entidad == null)
            throw new CustomException(TipoErrorEnum.NO_ENCONTRADO, "Producto no encontrado.");

        var dto = _mapper.Map<ProductoDto>(entidad);

        _logger.LogsEnd("App", "GetProductoPorId", dto);
        return dto;
    }

    public async Task<RespuestaDto<int>> CrearProductoAsync(ProductoCrearDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "CrearProducto", body);

        var entidad = _mapper.Map<Producto>(body);
        entidad.UsuarioCreacion = usuario;

        var id = await _repo.InsertarAsync(entidad, ct);

        if (id <= 0)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo crear el producto.");

        var resp = new RespuestaDto<int>
        {
            IsSuccess = true,
            Message = "Producto creado correctamente.",
            Data = id
        };

        _logger.LogsEnd("App", "CrearProducto", resp);
        return resp;
    }

    public async Task<RespuestaDto<bool>> ActualizarProductoAsync(ProductoActualizarDto body, string? usuario, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "ActualizarProducto", body);

        var entidad = _mapper.Map<Producto>(body);
        entidad.UsuarioModificacion = usuario;

        var ok = await _repo.ActualizarAsync(entidad, ct);

        if (!ok)
            throw new CustomException(TipoErrorEnum.SOLICITUD_INVALIDA, "No se pudo actualizar el producto.");

        var resp = new RespuestaDto<bool>
        {
            IsSuccess = true,
            Message = "Producto actualizado correctamente.",
            Data = true
        };

        _logger.LogsEnd("App", "ActualizarProducto", resp);
        return resp;
    }
}
