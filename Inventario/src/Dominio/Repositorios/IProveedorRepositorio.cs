using Dominio.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface IProveedorRepositorio
    {
        Task<List<Proveedor>> BuscarAsync(string? texto, CancellationToken ct = default);
        Task<Proveedor?> ObtenerPorIdAsync(int idProveedor, CancellationToken ct = default);

        Task<int> InsertarAsync(Proveedor entidad, CancellationToken ct = default);
        Task<bool> ActualizarAsync(Proveedor entidad, CancellationToken ct = default);
    }
}
