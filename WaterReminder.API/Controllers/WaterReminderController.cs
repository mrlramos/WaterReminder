using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WaterReminder.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "employee,manager")]
    public class WaterReminderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Este é um endpoint protegido");
        }
    }
}
