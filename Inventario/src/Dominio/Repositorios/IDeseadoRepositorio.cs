using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface IDeseadoRepositorio
    {
        Task<bool> ExisteAsync(int idUsuario, int idProducto, CancellationToken ct = default);
        Task<List<(int idProducto, DateTime fechaAgregadoUtc)>> ObtenerPorUsuarioAsync(int idUsuario, CancellationToken ct = default);

        Task<bool> ToggleAsync(int idUsuario, int idProducto, CancellationToken ct = default);
    }
}
