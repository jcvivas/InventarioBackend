using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Modelos
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public int? IdCategoria { get; set; }
        public string? UrlImagen { get; set; }
        public bool Activo { get; set; }
    }

    public class ProductoCrearDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public int? IdCategoria { get; set; }
        public string? UrlImagen { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class ProductoActualizarDto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public int? IdCategoria { get; set; }
        public string? UrlImagen { get; set; }
        public bool Activo { get; set; } = true;
    }
}
