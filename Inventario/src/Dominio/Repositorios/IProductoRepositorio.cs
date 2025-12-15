using Dominio.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface IProductoRepositorio
    {
        Task<List<Producto>> BuscarAsync(string? texto, CancellationToken ct = default);
        Task<Producto?> ObtenerPorIdAsync(int idProducto, CancellationToken ct = default);

        Task<int> InsertarAsync(Producto entidad, CancellationToken ct = default);
        Task<bool> ActualizarAsync(Producto entidad, CancellationToken ct = default);
    }
}
