using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public int? IdCategoria { get; set; }
        public string? UrlImagen { get; set; }

        public bool Activo { get; set; }

        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacionUtc { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] VersionFila { get; set; } = null!;

        public Categoria? Categoria { get; set; }
        public ICollection<ProductoProveedorLote> Ofertas { get; set; } = new List<ProductoProveedorLote>();
        public ICollection<Deseado> Deseados { get; set; } = new List<Deseado>();
    }
}
