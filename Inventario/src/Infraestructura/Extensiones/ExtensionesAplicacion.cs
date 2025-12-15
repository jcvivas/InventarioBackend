using Dominio.Modelos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infraestructura.Extensiones
{
    [ExcludeFromCodeCoverage]
    public static class ExtensionesAplicacion
    {
        public static string GetEnumMemberValue<T>(this T enumValue) where T : Enum
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString());

            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((EnumMemberAttribute)attributes[0]).Value ?? enumValue.ToString();
                }
            }

            return enumValue.ToString();
        }

        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var ex = feature?.Error;

                    var respuesta = new ErrorResponseDto();

                    try
                    {
                        if (ex is CustomException custom)
                        {
                            //segn el tipo de error
                            context.Response.StatusCode = custom.TipoError switch
                            {
                                TipoErrorEnum.TOKEN_NO_VALIDO => StatusCodes.Status401Unauthorized,
                                TipoErrorEnum.NO_ENCONTRADO => StatusCodes.Status404NotFound,
                                TipoErrorEnum.SOLICITUD_INVALIDA => StatusCodes.Status400BadRequest,
                                _ => StatusCodes.Status500InternalServerError
                            };

                            respuesta.Code = custom.TipoError.GetEnumMemberValue();
                            var esInterno = custom.TipoError == TipoErrorEnum.ERROR_INTERNO;
                            respuesta.Message = esInterno
                                ? MensajeGenerico(TipoErrorEnum.ERROR_INTERNO)
                                : (string.IsNullOrWhiteSpace(custom.Message) ? MensajeGenerico(custom.TipoError) : custom.Message);
                            respuesta.Message = string.IsNullOrWhiteSpace(custom.Message)
                                ? MensajeGenerico(custom.TipoError)
                                : custom.Message;

                            // detlle
                            if (custom.Errores is { Count: > 0 })
                                respuesta.Errors = custom.Errores;

                            if (context.Response.StatusCode >= 500)
                                Log.Error(ex, "Error controlado (CustomException) en {Path}", feature?.Path);
                            else
                                Log.Warning(ex, "Error de solicitud (CustomException) en {Path}", feature?.Path);
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                            respuesta.Code = TipoErrorEnum.ERROR_INTERNO.GetEnumMemberValue();
                            respuesta.Message = MensajeGenerico(TipoErrorEnum.ERROR_INTERNO);

                            Log.Error(ex, "Excepción no controlada en {Path}", feature?.Path);
                        }
                    }
                    catch (Exception handlerEx)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        respuesta.Code = TipoErrorEnum.ERROR_INTERNO.GetEnumMemberValue();
                        respuesta.Message = MensajeGenerico(TipoErrorEnum.ERROR_INTERNO);

                        Log.Error(handlerEx, "Fallo en ConfigureExceptionHandler");
                    }

                    var json = JsonSerializer.Serialize(respuesta, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false
                    });

                    await context.Response.WriteAsync(json);
                    await context.Response.Body.FlushAsync();
                });
            });

            return app;
        }

        private static string MensajeGenerico(TipoErrorEnum tipo)
        {
            return tipo switch
            {
                TipoErrorEnum.TOKEN_NO_VALIDO => "No autorizado.",
                TipoErrorEnum.NO_ENCONTRADO => "No se encontraron datos.",
                TipoErrorEnum.SOLICITUD_INVALIDA => "La solicitud no es válida.",
                _ => "Estamos presentando un inconveniente. Por favor, inténtalo de nuevo en unos minutos."
            };
        }
    }

}
