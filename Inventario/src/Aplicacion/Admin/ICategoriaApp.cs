using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Admin
{
    public interface ICategoriaApp
    {
        Task<List<CategoriaDto>> GetCategoriasAsync(CancellationToken ct = default);
        Task<CategoriaDto?> GetCategoriaPorIdAsync(int idCategoria, CancellationToken ct = default);
        Task<RespuestaDto<int>> CrearCategoriaAsync(CategoriaCrearDto body, string? usuario, CancellationToken ct = default);
        Task<RespuestaDto<bool>> ActualizarCategoriaAsync(CategoriaActualizarDto body, string? usuario, CancellationToken ct = default);
    }
}
