using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Modelos
{
    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class CategoriaCrearDto
    {
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }

    public class CategoriaActualizarDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}
