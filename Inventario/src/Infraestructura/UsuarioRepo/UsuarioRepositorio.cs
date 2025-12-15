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

namespace Infraestructura.UsuarioRepo
{
    public class UsuarioRepositorio : RepositorioBase, IUsuarioRepositorio
    {
        private readonly ILogger<UsuarioRepositorio> _logger;
        public UsuarioRepositorio(AppDbContext context, IConfiguration config, ILogger<UsuarioRepositorio> logger)
        {
            SetRepoInit(context, config);
            _logger = logger;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int idUsuario, CancellationToken ct = default)
        {
            _logger.LogsInit("Obtener por Id", "Repo");
            var json = $"{{\"id_usuario\":{idUsuario}}}";

            var p1 = new SqlParameter("@opcion", "por_id");
            var p2 = new SqlParameter("@json", json);

            var lista = await _context.Set<Usuario>()
                .FromSqlRaw("EXEC seguridad.sp_cons_usuarios @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            _logger.LogsEnd("Obtener por Id", "Repo");

            return lista.FirstOrDefault();
        }


        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken ct = default)
        {
            _logger.LogsInit("Obtener por Correo", "Repo");
            var json = $"{{\"correo\":\"{correo}\"}}";
            var p1 = new SqlParameter("@opcion", "por_correo");
            var p2 = new SqlParameter("@json", json);

            var resp = await _context.Set<Usuario>()
                .FromSqlRaw("EXEC seguridad.sp_cons_usuarios @opcion, @json", p1, p2)
                .AsNoTracking()
                .ToListAsync(ct);

            _logger.LogsEnd("Obtener por Correo", "Repo");

            return resp.FirstOrDefault()    ;
        }
    }
}
