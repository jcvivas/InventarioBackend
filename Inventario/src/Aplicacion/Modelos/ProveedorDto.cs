using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Modelos
{
    public class ProveedorDto
    {
        public int IdProveedor { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Identificacion { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
    }

    public class ProveedorCrearDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Identificacion { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class ProveedorActualizarDto
    {
        public int IdProveedor { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Identificacion { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;
    }
}
