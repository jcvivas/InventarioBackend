using Dominio.Repositorios;
using Infraestructura.CategoriaRepo;
using Infraestructura.CompraRepo;
using Infraestructura.Context;
using Infraestructura.DeseadoRepo;
using Infraestructura.MovimientoInventarioRepo;
using Infraestructura.PedidoRepo;
using Infraestructura.ProductoProveedorLoteRepo;
using Infraestructura.ProductoRepo;
using Infraestructura.ProveedorRepo;
using Infraestructura.UsuarioRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infraestructura.Ioc
{
    [ExcludeFromCodeCoverage]
    public static class DIInfraestructura
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var cadenaConexion = configuration.GetConnectionString("SqlServer");

            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(cadenaConexion, sql =>
                {
                    sql.EnableRetryOnFailure();
                    sql.CommandTimeout(60);
                });

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
            services.AddScoped<IProveedorRepositorio, ProveedorRepositorio>();
            services.AddScoped<IProductoProveedorLoteRepositorio, ProductoProveedorLoteRepositorio>();
            services.AddScoped<IDeseadoRepositorio, DeseadoRepositorio>();
            services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
            services.AddScoped<IMovimientoInventarioRepositorio, MovimientoInventarioRepositorio>();
            services.AddScoped<ICompraRepositorio, CompraRepositorio>();


            return services;
        }
    }
}
