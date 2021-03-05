using Microsoft.AspNetCore.Mvc;

namespace WeatherIdentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NullController : Controller
    {
        [HttpGet]
        public IActionResult GetSomething()
        {
            return NotFound();
        }
    }
}
