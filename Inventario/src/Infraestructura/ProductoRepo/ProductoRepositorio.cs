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

namespace Infraestructura.ProductoRepo
{
    public class ProductoRepositorio : RepositorioBase, IProductoRepositorio
    {
        private readonly ILogger<ProductoRepositorio> _logger;
        public ProductoRepositorio(AppDbContext context, IConfiguration config, ILogger<ProductoRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<List<Producto>> BuscarAsync(string? texto, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Buscar Productos");
            var json = JsonSerializer.Serialize(new { texto = texto ?? "" });

            var p1 = new SqlParameter("@opcion", "buscar");
            var p2 = new SqlParameter("@json", json);

            _logger.LogsEnd("Repositorio", "Buscar Productos");

            return await _context.Set<Producto>()
                .FromSqlRaw("EXEC inventario.sp_cons_productos @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Producto?> ObtenerPorIdAsync(int idProducto, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Producto por Id");
            var json = $"{{\"id_producto\":{idProducto}}}";

            var p1 = new SqlParameter("@opcion", "por_id");
            var p2 = new SqlParameter("@json", json);

            var lista = await _context.Set<Producto>()
                .FromSqlRaw("EXEC inventario.sp_cons_productos @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
            _logger.LogsEnd("Repositorio", "Obtener Producto por Id");
            return lista.FirstOrDefault();
        }

        public async Task<int> InsertarAsync(Producto entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Insertar Producto");
            var json = JsonSerializer.Serialize(new
            {
                codigo = entidad.Codigo,
                nombre = entidad.Nombre,
                descripcion = entidad.Descripcion,
                marca = entidad.Marca,
                id_categoria = entidad.IdCategoria,
                url_imagen = entidad.UrlImagen,
                activo = entidad.Activo,
                usuario_creacion = entidad.UsuarioCreacion
            });

            var ok = await EjecutarUpsertAsync("inventario.sp_upsert_productos", "insertar", json, ct);
            if (!ok) return 0;

            var encontrados = await BuscarAsync(entidad.Codigo, ct);
            var exacto = encontrados.FirstOrDefault(x => x.Codigo == entidad.Codigo);
            _logger.LogsEnd("Repositorio", "Insertar Producto");
            return exacto?.IdProducto ?? 0;
        }

        public async Task<bool> ActualizarAsync(Producto entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Actualizar Producto");
            var json = JsonSerializer.Serialize(new
            {
                id_producto = entidad.IdProducto,
                codigo = entidad.Codigo,
                nombre = entidad.Nombre,
                descripcion = entidad.Descripcion,
                marca = entidad.Marca,
                id_categoria = entidad.IdCategoria,
                url_imagen = entidad.UrlImagen,
                activo = entidad.Activo,
                usuario_modificacion = entidad.UsuarioModificacion
            });
            _logger.LogsEnd("Repositorio", "Actualizar Producto");

            return await EjecutarUpsertAsync("inventario.sp_upsert_productos", "actualizar", json, ct);
        }

        private async Task<bool> EjecutarUpsertAsync(string sp, string opcion, string json, CancellationToken ct)
        {
            _logger.LogsInit("Repositorio", "Ejecutar Upsert Producto");
            var p1 = new SqlParameter("@opcion", opcion);
            var p2 = new SqlParameter("@json", json);

            var resp = await _context.Set<ResultadoOperacionSp>()
                .FromSqlRaw($"EXEC {sp} @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
            _logger.LogsEnd("Repositorio", "Ejecutar Upsert Producto");

            return resp.Count > 0 && resp[0].IsSuccess;
        }
    }

}
