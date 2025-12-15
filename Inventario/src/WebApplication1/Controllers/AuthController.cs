using Aplicacion.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Tags("Auth")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _log;
        private readonly IAuthApp _app;

        public AuthController(ILogger<AuthController> log, IAuthApp app)
        {
            _log = log;
            _app = app;
        }

        public class LoginDto
        {
            public string Correo { get; set; } = string.Empty;
            public string Contrasena { get; set; } = string.Empty;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
        {
            using (_log.BeginScope("Login -> {correo}", dto.Correo))
            {
                var resp = await _app.LoginAsync(dto.Correo, dto.Contrasena, ct);
                return Ok(resp);
            }
        }
    }
}
