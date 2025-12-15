using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class ProductoProveedorLote
    {
        public int IdProductoProveedorLote { get; set; }

        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }

        public string? NumeroLote { get; set; }

        public string NumeroLoteNormalizado { get; private set; } = null!;

        public decimal PrecioUnitario { get; set; }
        public int StockDisponible { get; set; }
        public int StockReservado { get; set; }

        public string Moneda { get; set; } = "USD";
        public DateTime? FechaVencimiento { get; set; }

        public bool Activo { get; set; }

        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacionUtc { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] VersionFila { get; set; } = null!;

        public Producto Producto { get; set; } = null!;
        public Proveedor Proveedor { get; set; } = null!;
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
        public ICollection<PedidoDetalle> DetallesPedido { get; set; } = new List<PedidoDetalle>();
    }
}
