using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class PedidoDetalle
    {
        public long IdPedidoDetalle { get; set; }
        public long IdPedido { get; set; }
        public int IdProductoProveedorLote { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; private set; }

        public Pedido Pedido { get; set; } = null!;
        public ProductoProveedorLote ProductoProveedorLote { get; set; } = null!;
    }
}
