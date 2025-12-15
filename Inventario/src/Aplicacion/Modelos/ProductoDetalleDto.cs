using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Modelos
{
    public class ProductoDetalleDto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public int? IdCategoria { get; set; }
        public string? Categoria { get; set; }
        public string? UrlImagen { get; set; }
        public bool Activo { get; set; }

        public bool EsDeseado { get; set; }

        public List<OfertaDetalleDto> Ofertas { get; set; } = new();
    }

    public class OfertaDetalleDto
    {
        public int IdProductoProveedorLote { get; set; }
        public int IdProveedor { get; set; }
        public string? CodigoProveedor { get; set; }
        public string? NombreProveedor { get; set; }

        public string? NumeroLote { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int StockParaVenta { get; set; }
        public string? Moneda { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool OfertaActiva { get; set; }
    }

}
