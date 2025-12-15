using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Modelos
{
    public class OfertaDto
    {
        public int IdProductoProveedorLote { get; set; }
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public string? NumeroLote { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int StockDisponible { get; set; }
        public int StockReservado { get; set; }
        public string Moneda { get; set; } = "USD";
        public DateTime? FechaVencimiento { get; set; }
        public bool Activo { get; set; }
    }

    public class OfertaCrearDto
    {
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public string? NumeroLote { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int StockDisponible { get; set; }
        public int StockReservado { get; set; } = 0;
        public string Moneda { get; set; } = "USD";
        public DateTime? FechaVencimiento { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class OfertaActualizarDto
    {
        public int IdProductoProveedorLote { get; set; }
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public string? NumeroLote { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int StockDisponible { get; set; }
        public int StockReservado { get; set; }
        public string Moneda { get; set; } = "USD";
        public DateTime? FechaVencimiento { get; set; }
        public bool Activo { get; set; } = true;
    }
}
