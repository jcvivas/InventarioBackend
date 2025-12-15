using Dominio.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface ICategoriaRepositorio
    {
        Task<List<Categoria>> ObtenerTodasAsync(CancellationToken ct = default);
        Task<Categoria?> ObtenerPorIdAsync(int idCategoria, CancellationToken ct = default);
        Task<int> InsertarAsync(Categoria entidad, CancellationToken ct = default);
        Task<bool> ActualizarAsync(Categoria entidad, CancellationToken ct = default);
    }
}
