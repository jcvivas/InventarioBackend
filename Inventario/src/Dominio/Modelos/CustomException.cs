using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    public class CustomException : Exception
    {
        public TipoErrorEnum TipoError { get; }
        public List<ErrorDto> Errores { get; }

        public CustomException(TipoErrorEnum tipoError, string message, List<ErrorDto>? errores = null)
            : base(message)
        {
            TipoError = tipoError;
            Errores = errores ?? new List<ErrorDto>();
        }
    }
}
