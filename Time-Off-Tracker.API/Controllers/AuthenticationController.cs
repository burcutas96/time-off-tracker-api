﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.DTO.Concrete;
using Time_Off_Tracker.Entity.Concrete;

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
                
                var existingUser = _userService.SGetByEmail(userForRegisterDto.UserEmail);
                if ( existingUser != null)
                {
                    return Conflict("Girdiğiniz mail zaten mevcut, giriş yapın!");
                }


                User user = new()
                {
                    UserName = userForRegisterDto.UserName,
                    UserLastName = userForRegisterDto.UserLastName,
                    UserEmail = userForRegisterDto.UserEmail,
                    UserPassword = userForRegisterDto.UserPassword,
                    UserCreateDate = DateTime.Now.ToUniversalTime(),
                    UserRole = "Employee",
                    UserStatus = true
                };

                var passwordHasher = new PasswordHasher<User>();
                user.UserPassword = passwordHasher.HashPassword(user, user.UserPassword);

                // Kullanıcıyı veritabanına ekleyin
                _userService.SInsert(user);

                var token = GenerateJwtToken(user);
                return Ok(
                    new { Message = "Kullanıcı başarıyla kaydedildi." }
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
            var users = _userService.SGetByEmail(userForLoginDto.UserEmail);

            if (users == null)
            {
                return NotFound("Kullanıcı bulunamadı");
            }



            var passwordHasher = new PasswordHasher<UserForLoginDto>();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(userForLoginDto, users.UserPassword, userForLoginDto.UserPassword);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return BadRequest("Hatalı parola");
            }

            var token = GenerateJwtToken(users);

            return Ok(new { token });
        }


        private string GenerateJwtToken(User user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer, audience: audience,
expires: expiry, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}