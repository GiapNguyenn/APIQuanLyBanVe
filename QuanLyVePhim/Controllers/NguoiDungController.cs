using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanLyVePhim.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyVePhim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly ApplicationDbContext _NguoiDung;
        private readonly IConfiguration _configuration;
        public NguoiDungController(ApplicationDbContext NguoiDung, IConfiguration configuration)
        {
            _NguoiDung = NguoiDung;
            _configuration = configuration;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] NguoiDung user)
        {
            try
            {
                // Thực thi stored procedure và đọc kết quả
                var result = await _NguoiDung.Database.ExecuteSqlInterpolatedAsync($"EXEC TNG_Login2 {user.userName}, {user.Password}");

                // Kiểm tra kết quả trả về từ stored procedure
                if (result ==-1)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }
                else
                {
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ nếu cần
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }
        private string GenerateJwtToken(NguoiDung user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.userName),
            new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:ValidIssuer"],
                Audience = _configuration["Jwt:ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            List<NguoiDung> listNguoiDung = null;
            listNguoiDung = await _NguoiDung.Set<NguoiDung>().FromSqlInterpolated($"Exec timkiem").ToListAsync();
            return Ok(listNguoiDung);
        }

    }
}
