using Dominio.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repositorios
{
    public interface ICompraRepositorio
    {
        Task<SpProcesarCompraResultado?> ProcesarCompraAsync(string json, CancellationToken ct = default);
    }
}
