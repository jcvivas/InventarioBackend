using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Comprador
{
    public interface IProductoCompradorApp
    {
        Task<List<ProductoBusquedaDto>> BuscarAsync(string texto, int? idUsuario, CancellationToken ct = default);
        Task<ProductoDetalleDto?> ObtenerDetalleAsync(int idProducto, int? idUsuario, CancellationToken ct = default);
    }
}
