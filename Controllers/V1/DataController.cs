using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SassRefactorApi.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DataController : ControllerBase
    {
        [HttpGet("Getdata")]
        public async Task<ActionResult> Index()
        {
            return Ok("Token válido, acceso permitido.");
        }
    }
}