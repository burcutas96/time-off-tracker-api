using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.Entity.Concrete;
using Time_Off_Tracker.Entity.Dto;

namespace Time_Off_Tracker.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                // Kullanıcı kaydı mantığını uygulayın
                // Parolayı karma ve tuzlayın, veritabanına kaydedin
                // Başarılı yanıt döndürün

                // Kullanıcının veritabanında var olup olmadığını kontrol edin
                var existingUserName = _userService.SGetByUsername(userForRegisterDto.FirstName);
                var existingUser = _userService.SGetByEmail(userForRegisterDto.Email);
                if (existingUserName != null || existingUser != null)
                {
                    return Conflict("Girdiğiniz kullanıcı zaten mevcut, giriş yapın!");
                }


                User user = new()
                {
                    UserName = userForRegisterDto.FirstName,
                    UserLastName = userForRegisterDto.LastName,
                    UserEmail = userForRegisterDto.Email,
                    UserPassword = userForRegisterDto.Password,
                    UserCreateDate = DateTime.Now,
                    UserRole = "Employee",
                    UserStatus = true
                };

                var passwordHasher = new PasswordHasher<User>();
                user.UserPassword = passwordHasher.HashPassword(user, user.UserPassword);

                // Kullanıcıyı veritabanına ekleyin
                _userService.SInsert(user);

                var token = GenerateJwtToken(user);
                return Ok(
                    new { Message = "Kullanıcı başarıyla kaydedildi.", Token = token }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var users = _userService.SGetByEmail(userForLoginDto.Email);

            if (users == null)
            {
                return NotFound("Kullanıcı bulunamadı");
            }



            var passwordHasher = new PasswordHasher<UserForLoginDto>();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(userForLoginDto, users.UserPassword, userForLoginDto.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return BadRequest("Hatalı parola");
            }

            var token = GenerateJwtToken(users);

            return Ok(new { token });
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
                // Diğer talepleri burada ekleyebilirsiniz
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15), // Token süresi
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}