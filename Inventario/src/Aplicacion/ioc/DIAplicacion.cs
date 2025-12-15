using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Aplicacion.Admin;
using Aplicacion.Comprador;
using Aplicacion.Auth;


namespace Aplicacion.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DIAplicacion
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICategoriaApp, CategoriaApp>();
            services.AddScoped<IProductoApp, ProductoApp>();
            services.AddScoped<IProveedorApp, ProveedorApp>();
            services.AddScoped<IOfertaApp, OfertaApp>(); 

            services.AddScoped<IProductoCompradorApp, ProductoCompradorApp>(); 
            services.AddScoped<IDeseadoApp, DeseadoApp>();                     
            services.AddScoped<ICompraApp, CompraApp>();
            services.AddScoped<IMonitoreoInventarioApp, MonitoreoInventarioApp>();
            services.AddScoped<IEliminacionApp, EliminacionApp>();

            services.AddScoped<IAuthApp, AuthApp>(); 

            return services;
        }
    }
}
