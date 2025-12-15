using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aplicacion.Modelos;
using Aplicacion.Utils;
using Dominio.Modelos;
using Dominio.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Aplicacion.Auth;

public class AuthApp : IAuthApp
{
    private readonly ILogger<AuthApp> _logger;
    private readonly IUsuarioRepositorio _usuarioRepo;
    private readonly IConfiguration _config;

    public AuthApp(ILogger<AuthApp> logger, IUsuarioRepositorio usuarioRepo, IConfiguration config)
    {
        _logger = logger;
        _usuarioRepo = usuarioRepo;
        _config = config;
    }

    public async Task<RespuestaDto<string>> LoginAsync(string correo, string contrasena, CancellationToken ct = default)
    {
        _logger.LogsInit("App", "Login", new { correo });

        var usuario = await _usuarioRepo.ObtenerPorCorreoAsync(correo, ct);

        if (usuario == null || !usuario.Activo)
            throw new CustomException(TipoErrorEnum.TOKEN_NO_VALIDO, "Credenciales inválidas.");

        var ok = BCrypt.Net.BCrypt.Verify(contrasena, usuario.HashContrasena);
        if (!ok)
            throw new CustomException(TipoErrorEnum.TOKEN_NO_VALIDO, "Credenciales inválidas.");

        var token = GenerarJwt(usuario.IdUsuario, usuario.Correo, usuario.Rol);

        var resp = new RespuestaDto<string>
        {
            IsSuccess = true,
            Message = "Inicio de sesión exitoso.",
            Data = token
        };

        _logger.LogsEnd("App", "Login", new { usuario.IdUsuario, usuario.Rol });
        return resp;
    }

    private string GenerarJwt(int idUsuario, string correo, string rol)
    {
        var secret = _config["Jwt:Secret"];
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var minutos = int.TryParse(_config["Jwt:MinutosExpiracion"], out var m) ? m : 60;

        if (string.IsNullOrWhiteSpace(secret))
            throw new CustomException(TipoErrorEnum.ERROR_INTERNO, "JWT no configurado.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("id_usuario", idUsuario.ToString()),
            new Claim(ClaimTypes.Email, correo),
            new Claim(ClaimTypes.Role, rol)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutos),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
