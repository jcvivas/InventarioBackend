using Dominio.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface IMovimientoInventarioRepositorio
    {
        Task<long> InsertarAsync(MovimientoInventario movimiento, CancellationToken ct = default);
        Task<List<MovimientoInventario>> ObtenerPorProductoProveedorLoteAsync(int idProductoProveedorLote, CancellationToken ct = default);
    }
}
