using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class SpProcesarCompraResultado
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public long IdPedido { get; set; }
        public decimal Total { get; set; }
    }
}
