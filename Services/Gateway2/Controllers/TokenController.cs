using System.Threading.Tasks;
using Entities.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Gateway2.Controllers
{
    [Route("token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        
        private Service _customerService;

        public TokenController()
        {
            _customerService = new Service("customerservice", 80);
        }
        

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] AuthDto authDto)
        {
            var (response, _) = await _customerService.Route(HttpContext, bodyArg: authDto);
            return Ok(response);
        }
    }
}