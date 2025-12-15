using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    public enum TipoErrorEnum
    {
        [EnumMember(Value = "ERROR_INTERNO")]
        ERROR_INTERNO,

        [EnumMember(Value = "TOKEN_NO_VALIDO")]
        TOKEN_NO_VALIDO,

        [EnumMember(Value = "SOLICITUD_INVALIDA")]
        SOLICITUD_INVALIDA,

        [EnumMember(Value = "NO_ENCONTRADO")]
        NO_ENCONTRADO
    }
}
