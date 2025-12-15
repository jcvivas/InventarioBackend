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

namespace Infraestructura.ProductoProveedorLoteRepo
{
    public class ProductoProveedorLoteRepositorio : RepositorioBase, IProductoProveedorLoteRepositorio
    {
        private readonly ILogger<ProductoProveedorLoteRepositorio> _logger;
        public ProductoProveedorLoteRepositorio(AppDbContext context, IConfiguration config, ILogger<ProductoProveedorLoteRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<List<ProductoProveedorLote>> ObtenerPorProductoAsync(int idProducto, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Por producto");
            var json = JsonSerializer.Serialize(new { id_producto = idProducto });

            var p1 = new SqlParameter("@opcion", "por_producto");
            var p2 = new SqlParameter("@json", json);

            _logger.LogsEnd("Repositorio", "Obtener Por producto");

            return await _context.Set<ProductoProveedorLote>()
                .FromSqlRaw("EXEC inventario.sp_cons_producto_proveedor_lote @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<ProductoProveedorLote?> ObtenerPorIdAsync(int idProductoProveedorLote, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Por id");
            var json = JsonSerializer.Serialize(new { id_producto_proveedor_lote = idProductoProveedorLote });

            var p1 = new SqlParameter("@opcion", "por_id");
            var p2 = new SqlParameter("@json", json);

            _logger.LogsEnd("Repositorio", "Obtener Por id");
            var lista = await _context.Set<ProductoProveedorLote>()
                .FromSqlRaw("EXEC inventario.sp_cons_producto_proveedor_lote @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            return lista.FirstOrDefault();
        }

        public async Task<int> InsertarAsync(ProductoProveedorLote entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Insertar");
            var json = JsonSerializer.Serialize(new
            {
                id_producto = entidad.IdProducto,
                id_proveedor = entidad.IdProveedor,
                numero_lote = entidad.NumeroLote,
                precio_unitario = entidad.PrecioUnitario,
                stock_disponible = entidad.StockDisponible,
                stock_reservado = entidad.StockReservado,
                moneda = entidad.Moneda,
                fecha_vencimiento = entidad.FechaVencimiento,
                activo = entidad.Activo,
                usuario_creacion = entidad.UsuarioCreacion
            });

            var ok = await EjecutarUpsertAsync("inventario.sp_upsert_producto_proveedor_lote", "insertar", json, ct);
            if (!ok) return 0;

            var ofertas = await ObtenerPorProductoAsync(entidad.IdProducto, ct);
            var encontrada = ofertas
                .Where(x => x.IdProveedor == entidad.IdProveedor && x.NumeroLote == entidad.NumeroLote)
                .OrderByDescending(x => x.IdProductoProveedorLote)
                .FirstOrDefault();
            _logger.LogsEnd("Repositorio", "Insertar");

            return encontrada?.IdProductoProveedorLote ?? 0;
        }

        public async Task<bool> ActualizarAsync(ProductoProveedorLote entidad, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Actualizar");
            var json = JsonSerializer.Serialize(new
            {
                id_producto_proveedor_lote = entidad.IdProductoProveedorLote,
                id_producto = entidad.IdProducto,
                id_proveedor = entidad.IdProveedor,
                numero_lote = entidad.NumeroLote,
                precio_unitario = entidad.PrecioUnitario,
                stock_disponible = entidad.StockDisponible,
                stock_reservado = entidad.StockReservado,
                moneda = entidad.Moneda,
                fecha_vencimiento = entidad.FechaVencimiento,
                activo = entidad.Activo,
                usuario_modificacion = entidad.UsuarioModificacion
            });
            _logger.LogsEnd("Repositorio", "Actualizar");

            return await EjecutarUpsertAsync("inventario.sp_upsert_producto_proveedor_lote", "actualizar", json, ct);
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
