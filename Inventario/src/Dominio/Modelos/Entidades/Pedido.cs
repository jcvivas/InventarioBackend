using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class Pedido
    {
        public long IdPedido { get; set; }
        public int IdUsuario { get; set; }

        public string Estado { get; set; } = null!;
        public decimal Total { get; set; }
        public string Moneda { get; set; } = "USD";
        public DateTime FechaCreacionUtc { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}
