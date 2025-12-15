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


namespace Infraestructura.ProveedorRepo
{
    public class ProveedorRepositorio : RepositorioBase, IProveedorRepositorio
    {
        private readonly ILogger<ProveedorRepositorio> _logger;
        public ProveedorRepositorio(AppDbContext context, IConfiguration config, ILogger<ProveedorRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<List<Proveedor>> BuscarAsync(string? texto, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(new { texto = texto ?? "" });

            var p1 = new SqlParameter("@opcion", "buscar");
            var p2 = new SqlParameter("@json", json);

            return await _context.Set<Proveedor>()
                .FromSqlRaw("EXEC inventario.sp_cons_proveedores @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Proveedor?> ObtenerPorIdAsync(int idProveedor, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Proveedor Por id");
            var json = $"{{\"id_proveedor\":{idProveedor}}}";

            var p1 = new SqlParameter("@opcion", "por_id");
            var p2 = new SqlParameter("@json", json);

            var lista = await _context.Set<Proveedor>()
                .FromSqlRaw("EXEC inventario.sp_cons_proveedores @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            _logger.LogsEnd("Repositorio", "Obtener Proveedor Por id");

            return lista.FirstOrDefault();
        }

        public async Task<int> InsertarAsync(Proveedor entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Insertar Proveedor");
            var json = JsonSerializer.Serialize(new
            {
                codigo = entidad.Codigo,
                nombre = entidad.Nombre,
                identificacion = entidad.Identificacion,
                correo = entidad.Correo,
                telefono = entidad.Telefono,
                activo = entidad.Activo,
                usuario_creacion = entidad.UsuarioCreacion
            });

            var ok = await EjecutarUpsertAsync("inventario.sp_upsert_proveedores", "insertar", json, ct);
            if (!ok) return 0;

            var encontrados = await BuscarAsync(entidad.Codigo, ct);
            var exacto = encontrados.FirstOrDefault(x => x.Codigo == entidad.Codigo);
            _logger.LogsEnd("Repositorio", "Insertar Proveedor");
            return exacto?.IdProveedor ?? 0;
        }

        public async Task<bool> ActualizarAsync(Proveedor entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Actualizar Proveedor");
            var json = JsonSerializer.Serialize(new
            {
                id_proveedor = entidad.IdProveedor,
                codigo = entidad.Codigo,
                nombre = entidad.Nombre,
                identificacion = entidad.Identificacion,
                correo = entidad.Correo,
                telefono = entidad.Telefono,
                activo = entidad.Activo,
                usuario_modificacion = entidad.UsuarioModificacion
            });
            _logger.LogsEnd("Repositorio", "Actualizar Proveedor");

            return await EjecutarUpsertAsync("inventario.sp_upsert_proveedores", "actualizar", json, ct);
        }

        private async Task<bool> EjecutarUpsertAsync(string sp, string opcion, string json, CancellationToken ct)
        {
            _logger.LogsInit("Repositorio", "Ejecutar Upsert");
            var p1 = new SqlParameter("@opcion", opcion);
            var p2 = new SqlParameter("@json", json);

            var resp = await _context.Set<ResultadoOperacionSp>()
                .FromSqlRaw($"EXEC {sp} @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
            _logger.LogsEnd("Repositorio", "Ejecutar Upsert");
            return resp.Count > 0 && resp[0].IsSuccess;
        }
    }
}
