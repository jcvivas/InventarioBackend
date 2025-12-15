using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Admin
{
    public interface IProveedorApp
    {
        Task<List<ProveedorDto>> BuscarProveedoresAsync(string? texto, CancellationToken ct = default);
        Task<ProveedorDto?> GetProveedorPorIdAsync(int idProveedor, CancellationToken ct = default);
        Task<RespuestaDto<int>> CrearProveedorAsync(ProveedorCrearDto body, string? usuario, CancellationToken ct = default);
        Task<RespuestaDto<bool>> ActualizarProveedorAsync(ProveedorActualizarDto body, string? usuario, CancellationToken ct = default);
    }
}
