using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.Entity.Concrete;

namespace Time_Off_Tracker.API.Controllers
{
    [Route("users")] // İstenilen URL yapısı
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UserUpdate(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
    
            return Ok();
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateUser(int id)
        {
            return Ok();
        }

        [HttpPost("password-changes")]
        public IActionResult AddUser(User user)
        {
            _userService.SInsert(user);
            return Ok();
        }

        [HttpGet]
        public IActionResult UserList()
        {
            var values =  _userService.SGetList();
            return Ok(values);
        }


    }
}
