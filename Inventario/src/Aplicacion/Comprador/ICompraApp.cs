using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Comprador
{
    public interface ICompraApp
    {
        Task<RespuestaDto<CompraResultadoDto>> ProcesarCompraAsync(CompraCrearDto body, CancellationToken ct = default);
    }
}
