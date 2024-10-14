using Microsoft.AspNetCore.Mvc;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Models.Auth.Students;

namespace SassRefactorApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthServices _services;
        public LoginController(IAuthServices services)
        {
            _services = services;
        }
        [HttpPost("Student")]
        public async Task<ActionResult> Login(LoginRequestDTO login)
        {
            VOToken token = await _services.AttemptLogin(login.User, login.Password);
            if (token != null)
            {
                return Ok(token);
            }
            return BadRequest(token);
        }
    }
}
