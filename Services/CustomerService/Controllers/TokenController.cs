using System.Threading.Tasks;
using Entities.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [Route("token")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private IApplicationService _applicationService;

        public TokenController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken(AuthDto authDto)
        {
            var token = await _applicationService.CreateToken(authDto);
            return Ok(token);
        }
    }
}