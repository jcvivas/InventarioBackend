using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Admin
{
    public interface IOfertaApp
    {
        Task<List<OfertaDto>> GetOfertasPorProductoAsync(int idProducto, CancellationToken ct = default);
        Task<OfertaDto?> GetOfertaPorIdAsync(int idProductoProveedorLote, CancellationToken ct = default);
        Task<RespuestaDto<int>> CrearOfertaAsync(OfertaCrearDto body, string? usuario, CancellationToken ct = default);
        Task<RespuestaDto<bool>> ActualizarOfertaAsync(OfertaActualizarDto body, string? usuario, CancellationToken ct = default);
    }
}
