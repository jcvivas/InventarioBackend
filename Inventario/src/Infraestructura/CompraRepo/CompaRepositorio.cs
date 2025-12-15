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
using System.Threading.Tasks;

namespace Infraestructura.CompraRepo
{
    public class CompraRepositorio : RepositorioBase, ICompraRepositorio
    {
        private readonly ILogger<CompraRepositorio> _logger;

        public CompraRepositorio(AppDbContext context, IConfiguration config, ILogger<CompraRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<SpProcesarCompraResultado?> ProcesarCompraAsync(string json, CancellationToken ct = default)
        {
            _logger.LogsInit("Repositorio", "ProcesarCompra", new { json });

            var pOpcion = new SqlParameter("@opcion", "procesar");
            var pJson = new SqlParameter("@json", json);

            var lista = await _context.Set<SpProcesarCompraResultado>()
                .FromSqlRaw("EXEC ventas.sp_procesar_compra @opcion=@opcion, @json=@json", pOpcion, pJson)
                .AsNoTracking()
                .ToListAsync(ct);

            var result = lista.FirstOrDefault();

            _logger.LogsEnd("Repositorio", "ProcesarCompra", result);
            return result;
        }


    }
}
