using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Identificacion { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }

        public bool Activo { get; set; }

        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacionUtc { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] VersionFila { get; set; } = null!;

        public ICollection<ProductoProveedorLote> Ofertas { get; set; } = new List<ProductoProveedorLote>();
    }
}
