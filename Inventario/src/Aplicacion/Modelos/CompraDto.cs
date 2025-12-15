using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Modelos
{
    public class CompraCrearDto
    {
        public int IdUsuario { get; set; }
        public string Moneda { get; set; } = "USD";
        public List<CompraDetalleDto> Detalles { get; set; } = new();
    }

    public class CompraDetalleDto
    {
        public int IdProductoProveedorLote { get; set; }
        public int Cantidad { get; set; }
    }

    public class CompraResultadoDto
    {
        public long IdPedido { get; set; }
        public decimal Total { get; set; }
    }
}
