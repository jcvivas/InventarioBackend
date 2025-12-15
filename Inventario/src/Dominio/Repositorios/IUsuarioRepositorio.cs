using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;

namespace Dominio.Repositorios

{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> ObtenerPorCorreoAsync(string correo, CancellationToken ct = default);
        Task<Usuario?> ObtenerPorIdAsync(int idUsuario, CancellationToken ct = default);
    }
}
