using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Comprador
{
    public interface IDeseadoApp
    {
        Task<List<DeseadoDto>> GetDeseadosAsync(int idUsuario, CancellationToken ct = default);
        Task<RespuestaDto<bool>> ToggleDeseadoAsync(int idUsuario, int idProducto, CancellationToken ct = default);
    }
}
