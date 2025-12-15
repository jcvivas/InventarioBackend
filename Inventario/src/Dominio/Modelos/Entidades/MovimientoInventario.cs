using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class MovimientoInventario
    {
        public long IdMovimiento { get; set; }
        public int IdProductoProveedorLote { get; set; }

        public string TipoMovimiento { get; set; } = null!;
        public int Cantidad { get; set; }
        public string? Motivo { get; set; }
        public string? Referencia { get; set; }

        public int? IdUsuario { get; set; }
        public DateTime FechaMovimientoUtc { get; set; }

        public ProductoProveedorLote ProductoProveedorLote { get; set; } = null!;
        public Usuario? Usuario { get; set; }
    }
}
