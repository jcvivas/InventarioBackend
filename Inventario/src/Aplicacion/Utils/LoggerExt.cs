using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aplicacion.Utils
{
    public static class LoggerExt
    {
        public static void LogsInit(this ILogger logger, string source, object metodo, object request = null, object headers = null)
        {
            logger.LogInformation("Incio {Src} {Metodo} , {Request} , {Headers}", source, metodo, JsonSerializer.Serialize(request), JsonSerializer.Serialize(headers));
        }

        public static void LogsEnd(this ILogger logger, string source, object metodo, object response = null, object statusCode = null)
        {
            logger.LogInformation("Fin {Src} {metodo} , {response} , {statusCode}", source, metodo, JsonSerializer.Serialize(response), statusCode);
        }
    }
}
