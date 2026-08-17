using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackManagement.Application.DTOs;

namespace TrackManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
      private readonly IConfiguration _configuration;

      public AuthController(IConfiguration configuration)
      {
            _configuration = configuration;
      }

      [HttpPost("login")]
      public IActionResult Login([FromBody] LoginDto dto)
      {
            if (dto.Username == "admin" && dto.Password == "password123")
            {
                  var token = GenerateJwtToken(dto.Username);
                  return Ok(new { token });
            }

            return Unauthorized(new { message = "بيانات الدخول غير صحيحة" });
      }

      private string GenerateJwtToken(string username)
      {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
      }
}
