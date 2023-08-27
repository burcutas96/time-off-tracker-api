using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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


        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var result= _userService.SGetById(id);
            return Ok(result);
            
        }


        // [HttpPut("{id}")]
        // public IActionResult UserUpdate(int id)
        // {
        //    return Ok();
        // }


        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var resultGet = _userService.SGetById(id);
            if (resultGet == null)
            {
                return BadRequest();
            }
            else
            {
                _userService.SDelete(resultGet);
                return Ok("Kullanıcı Başarıyla Silindi!");
            }
        }


        [Authorize]
        [HttpPut("update/")]
        public IActionResult UpdateUser(User user)
        {
            var passwordHasher = new PasswordHasher<User>();
            user.UserPassword = passwordHasher.HashPassword(user, user.UserPassword);

            _userService.SUpdate(user);
            return Ok("Kullanıcı Başarıyla Güncellendi!");
        }


        [Authorize]
        [HttpPost("add")]
        public IActionResult AddUser(User user)
        {
            _userService.SInsert(user);
            return Ok("Kullanıcı Başarıyla Eklendi!");
        }
        

        [HttpGet]
        public IActionResult UserList()
        {
            var values =  _userService.SGetList();
            return Ok(values);
        }


    }
}
