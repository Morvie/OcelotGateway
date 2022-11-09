using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OcelotAPIGateway
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public async Task<string> GetAll()
        {
            return "This is the Welcome action method...";
        }

    }
}
