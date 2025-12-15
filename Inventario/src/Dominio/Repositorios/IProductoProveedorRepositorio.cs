using Dominio.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface IProductoProveedorLoteRepositorio
    {
        Task<List<ProductoProveedorLote>> ObtenerPorProductoAsync(int idProducto, CancellationToken ct = default);
        Task<ProductoProveedorLote?> ObtenerPorIdAsync(int idProductoProveedorLote, CancellationToken ct = default);

        Task<int> InsertarAsync(ProductoProveedorLote entidad, CancellationToken ct = default);
        Task<bool> ActualizarAsync(ProductoProveedorLote entidad, CancellationToken ct = default);
    }
}
