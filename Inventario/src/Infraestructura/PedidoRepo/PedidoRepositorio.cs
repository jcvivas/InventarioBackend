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

namespace Infraestructura.PedidoRepo
{
    public class PedidoRepositorio : RepositorioBase, IPedidoRepositorio
    {
        private readonly ILogger<PedidoRepositorio> _logger;
        public PedidoRepositorio(AppDbContext context, IConfiguration config, ILogger<PedidoRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<long> InsertarAsync(Pedido pedido, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Insertar Pedido");
            var json = JsonSerializer.Serialize(new
            {
                id_usuario = pedido.IdUsuario,
                estado = pedido.Estado,
                total = pedido.Total,
                moneda = pedido.Moneda
            });

            var p1 = new SqlParameter("@opcion", "insertar");
            var p2 = new SqlParameter("@json", json);

            var resp = await _context.Set<ResultadoOperacionSp>()
                .FromSqlRaw("EXEC ventas.sp_upsert_pedidos @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            if (resp.Count == 0 || !resp[0].IsSuccess) return 0;

            var pedidos = await ObtenerPorUsuarioAsync(pedido.IdUsuario, ct);
            _logger.LogsEnd("Repositorio", "Insertar Pedido");
            return pedidos.FirstOrDefault()?.IdPedido ?? 0;
        }

        public async Task<List<Pedido>> ObtenerPorUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Pedidos por Usuario");
            var json = JsonSerializer.Serialize(new { id_usuario = idUsuario });

            var p1 = new SqlParameter("@opcion", "por_usuario");
            var p2 = new SqlParameter("@json", json);
            _logger.LogsEnd("Repositorio", "Obtener Pedidos por Usuario");
            return await _context.Set<Pedido>()
                .FromSqlRaw("EXEC ventas.sp_cons_pedidos @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Pedido?> ObtenerPorIdAsync(long idPedido, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "Obtener Pedido por Id");
            var pId = new SqlParameter("@id_pedido", idPedido);

            var cab = await _context.Set<Pedido>()
                .FromSqlRaw("SELECT * FROM ventas.pedidos WHERE id_pedido = @id_pedido", pId)
                .AsNoTracking()
                .ToListAsync(ct);

            var pedido = cab.FirstOrDefault();
            if (pedido is null) return null;


            var jsonDet = JsonSerializer.Serialize(new { id_pedido = idPedido });
            var p1 = new SqlParameter("@opcion", "por_pedido");
            var p2 = new SqlParameter("@json", jsonDet);

            var detalles = await _context.Set<PedidoDetalle>()
                .FromSqlRaw("EXEC ventas.sp_cons_pedido_detalles @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            pedido.Detalles = detalles;
            _logger.LogsEnd("Repositorio", "Obtener Pedido por Id");
            return pedido;
        }
    }
}
