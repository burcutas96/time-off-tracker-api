using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Time_Off_Tracker.API.Controllers
{
    [Route("time-off")]
    [ApiController]
    public class TimeOffController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult TimeOffGet(int id)
        {
            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult TimeOffUpdate(int id)
        {
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult TimeOffDelete(int id)
        {
            return Ok();
        }
        [HttpPut("upadate/{id}")]
        public IActionResult UpdateTimeOff(int id)
        {
            return Ok();
        }
        [HttpGet]
        public IActionResult TimeOffList()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult TimeOffAdd()
        {
            return Ok();
        }
    }
}
