using Aplicacion.IoC;
using Infraestructura.Ioc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Api.IoC
{
    public static class ServiceExtensions
    {
        private const string CorsPolicyName = "_CorsInventario";

        public static WebApplicationBuilder AddSwaggerCustom(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Inventario API",
                    Version = "v1",
                    Description = "API REST Inventario - Prueba Técnica"
                });

                var jwtScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Escribe: Bearer {tu_token}",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtScheme, Array.Empty<string>() }
                });

            });

            return builder;
        }

        public static WebApplicationBuilder SetCorsCustom(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return builder;
        }

        public static WebApplication UseCorsCustom(this WebApplication app)
        {
            app.UseCors(CorsPolicyName);
            return app;
        }

        public static WebApplication UseHttpPipelineCustom(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }

        public static WebApplicationBuilder SetRegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplication(builder.Configuration);

            builder.Services.AddInfrastructure(builder.Configuration);

            return builder;
        }
    }
}
