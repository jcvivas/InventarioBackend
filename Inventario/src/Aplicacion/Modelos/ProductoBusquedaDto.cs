using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Modelos
{
    public class ProductoBusquedaDto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public int? IdCategoria { get; set; }
        public string? Categoria { get; set; }
        public string? UrlImagen { get; set; }
        public bool Activo { get; set; }

        public decimal PrecioDesde { get; set; }
        public int StockTotalParaVenta { get; set; }
        public string? Moneda { get; set; }

        public bool EsDeseado { get; set; }
    }
}
