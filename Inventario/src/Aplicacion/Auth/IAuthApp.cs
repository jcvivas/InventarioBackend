using Aplicacion.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Auth
{
    public interface IAuthApp
    {
        Task<RespuestaDto<string>> LoginAsync(string correo, string contrasena, CancellationToken ct = default);
    }
}
