using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
