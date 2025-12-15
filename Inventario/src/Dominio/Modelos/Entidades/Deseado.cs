using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class Deseado
    {
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public DateTime FechaAgregadoUtc { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
    }
}
