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

namespace Infraestructura.MovimientoInventarioRepo
{
    public class MovimientoInventarioRepositorio : RepositorioBase, IMovimientoInventarioRepositorio
    {
        private readonly ILogger<MovimientoInventarioRepositorio> _logger;

        public MovimientoInventarioRepositorio(AppDbContext context, IConfiguration config, ILogger<MovimientoInventarioRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<long> InsertarAsync(MovimientoInventario movimiento, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Insertar Movimiento Inventario");
            var json = JsonSerializer.Serialize(new
            {
                id_producto_proveedor_lote = movimiento.IdProductoProveedorLote,
                tipo_movimiento = movimiento.TipoMovimiento,
                cantidad = movimiento.Cantidad,
                motivo = movimiento.Motivo,
                referencia = movimiento.Referencia,
                id_usuario = movimiento.IdUsuario
            });

            var p1 = new SqlParameter("@opcion", "insertar");
            var p2 = new SqlParameter("@json", json);

            var resp = await _context.Set<ResultadoOperacionSp>()
                .FromSqlRaw("EXEC inventario.sp_upsert_movimientos_inventario @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            if (resp.Count == 0 || !resp[0].IsSuccess) return 0;

            var lista = await ObtenerPorProductoProveedorLoteAsync(movimiento.IdProductoProveedorLote, ct);
            _logger.LogsEnd("Repositorio", "Insertar Movimiento Inventario");
            return lista.FirstOrDefault()?.IdMovimiento ?? 0;
        }

        public async Task<List<MovimientoInventario>> ObtenerPorProductoProveedorLoteAsync(int idProductoProveedorLote, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Movimientos por Producto Proveedor Lote");
            var json = JsonSerializer.Serialize(new { id_producto_proveedor_lote = idProductoProveedorLote });

            var p1 = new SqlParameter("@opcion", "por_ppl");
            var p2 = new SqlParameter("@json", json);

            _logger.LogsEnd("Repositorio", "Obtener Movimientos por Producto Proveedor Lote");

            return await _context.Set<MovimientoInventario>()
                .FromSqlRaw("EXEC inventario.sp_cons_movimientos_inventario @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }

}
