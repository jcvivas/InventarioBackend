using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; } = null!;
        public string HashContrasena { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public bool Activo { get; set; }

        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacionUtc { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] VersionFila { get; set; } = null!;

        public ICollection<Deseado> Deseados { get; set; } = new List<Deseado>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    }
}
