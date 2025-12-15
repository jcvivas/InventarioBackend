using Aplicacion.Utils;
using Dominio.Modelos.Entidades;
using Dominio.Repositorios;
using Infraestructura.Base;
using Infraestructura.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infraestructura.CategoriaRepo
{
    public class CategoriaRepositorio : RepositorioBase, ICategoriaRepositorio
    {
        private readonly ILogger<CategoriaRepositorio> _logger;
        public CategoriaRepositorio(AppDbContext context, IConfiguration config, ILogger<CategoriaRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<List<Categoria>> ObtenerTodasAsync(CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Todas Categorias");
            var p1 = new SqlParameter("@opcion", "todos");
            var p2 = new SqlParameter("@json", (object)DBNull.Value);
            _logger.LogsEnd("Repositorio", "Obtener Todas Categorias");

            return await _context.Set<Categoria>()
                .FromSqlRaw("EXEC catalogo.sp_cons_categorias @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int idCategoria, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Categoria Por id");
            var json = $"{{\"id_categoria\":{idCategoria}}}";

            var p1 = new SqlParameter("@opcion", "por_id");
            var p2 = new SqlParameter("@json", json);

            var lista = await _context.Set<Categoria>()
                .FromSqlRaw("EXEC catalogo.sp_cons_categorias @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
            _logger.LogsEnd("Repositorio", "Obtener Categoria Por id");

            return lista.FirstOrDefault();
        }

        public async Task<int> InsertarAsync(Categoria entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Insertar Categoria");
            var json = JsonSerializer.Serialize(new
            {
                nombre = entidad.Nombre,
                activo = entidad.Activo
            });

            var ok = await EjecutarUpsertAsync("catalogo.sp_upsert_categorias", "insertar", json, ct);
            if (!ok) return 0;

            // Recupero id por campo único: nombre
            var todas = await ObtenerTodasAsync(ct);
            var encontrada = todas.FirstOrDefault(x => x.Nombre == entidad.Nombre);
            _logger.LogsEnd("Repositorio", "Insertar Categoria");
            return encontrada?.IdCategoria ?? 0;
        }

        public async Task<bool> ActualizarAsync(Categoria entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Actualizar Categoria");
            var json = JsonSerializer.Serialize(new
            {
                id_categoria = entidad.IdCategoria,
                nombre = entidad.Nombre,
                activo = entidad.Activo
            });
            _logger.LogsEnd("Repositorio", "Actualizar Categoria");
            return await EjecutarUpsertAsync("catalogo.sp_upsert_categorias", "actualizar", json, ct);
        }

        private async Task<bool> EjecutarUpsertAsync(string sp, string opcion, string json, CancellationToken ct)
        {
            _logger.LogsInit("Repositorio", "Ejecutar Upsert Categoria");
            var p1 = new SqlParameter("@opcion", opcion);
            var p2 = new SqlParameter("@json", json);

            var resp = await _context.Set<ResultadoOperacionSp>()
                .FromSqlRaw($"EXEC {sp} @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            _logger.LogsEnd("Repositorio", "Ejecutar Upsert Categoria");

            return resp.Count > 0 && resp[0].IsSuccess;
        }
    }
}
