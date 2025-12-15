using Dominio.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface IPedidoRepositorio
    {
        Task<long> InsertarAsync(Pedido pedido, CancellationToken ct = default);
        Task<List<Pedido>> ObtenerPorUsuarioAsync(int idUsuario, CancellationToken ct = default);
        Task<Pedido?> ObtenerPorIdAsync(long idPedido, CancellationToken ct = default);
    }
}
