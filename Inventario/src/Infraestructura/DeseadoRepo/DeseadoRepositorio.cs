using Aplicacion.Utils;
using Dominio.Modelos.Entidades;
using Dominio.Repositorios;
using Infraestructura.Base;
using Infraestructura.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infraestructura.DeseadoRepo
{
    public class DeseadoRepositorio : RepositorioBase, IDeseadoRepositorio
    {
        private readonly ILogger<DeseadoRepositorio> _logger;
        public DeseadoRepositorio(AppDbContext context, IConfiguration config, ILogger<DeseadoRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;

        }

        public async Task<bool> ExisteAsync(int idUsuario, int idProducto, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Existe Producto");
            var json = JsonSerializer.Serialize(new { id_usuario = idUsuario, id_producto = idProducto });

            var p1 = new SqlParameter("@opcion", "por_clave");
            var p2 = new SqlParameter("@json", json);

            var lista = await _context.Set<Deseado>()
                .FromSqlRaw("EXEC ventas.sp_cons_deseados @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
            _logger.LogsEnd("Repsoitorio", "Existe Producto");

            return lista.Count > 0;
        }

        public async Task<List<(int idProducto, DateTime fechaAgregadoUtc)>> ObtenerPorUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener por usuario");
            var json = JsonSerializer.Serialize(new { id_usuario = idUsuario });

            var p1 = new SqlParameter("@opcion", "por_usuario");
            var p2 = new SqlParameter("@json", json);

            var lista = await _context.Set<Deseado>()
                .FromSqlRaw("EXEC ventas.sp_cons_deseados @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            _logger.LogsEnd("Repositorio", "Obtener por usuario");

            return lista.Select(x => (x.IdProducto, x.FechaAgregadoUtc)).ToList();
        }

        public async Task<bool> ToggleAsync(int idUsuario, int idProducto, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Toggle Deseado");
            var json = JsonSerializer.Serialize(new { id_usuario = idUsuario, id_producto = idProducto });

            var p1 = new SqlParameter("@opcion", "toggle");
            var p2 = new SqlParameter("@json", json);

            var resp = await _context.Set<ResultadoOperacionSp>()
                .FromSqlRaw("EXEC ventas.sp_toggle_deseado @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
            _logger.LogsEnd("Repositorio", "Toggle Deseado");

            return resp.Count > 0 && resp[0].IsSuccess;
        }
    }
}
