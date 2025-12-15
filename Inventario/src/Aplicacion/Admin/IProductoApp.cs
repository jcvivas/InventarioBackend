using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Admin
{

    public interface IProductoApp
    {
        Task<List<ProductoDto>> BuscarProductosAsync(string? texto, CancellationToken ct = default);
        Task<ProductoDto?> GetProductoPorIdAsync(int idProducto, CancellationToken ct = default);
        Task<RespuestaDto<int>> CrearProductoAsync(ProductoCrearDto body, string? usuario, CancellationToken ct = default);
        Task<RespuestaDto<bool>> ActualizarProductoAsync(ProductoActualizarDto body, string? usuario, CancellationToken ct = default);
    }
}
