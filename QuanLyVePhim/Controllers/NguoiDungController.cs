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
        public async Task<IActionResult> Login([FromBody] NguoiiDung user)
        {
            try
            {
                var parameters = new[]
   {
    new SqlParameter("@user", user.Name),
    new SqlParameter("@password", user.Password)
};

                var numberLogin =  _NguoiDung.Set<NguoiiDung>()
                    .FromSqlRaw("EXEC [dbo].[TNG_Login] @user, @password", parameters)
                    .AsEnumerable()
                    .FirstOrDefault();
                // Kiểm tra kết quả trả về từ stored procedure
                if (numberLogin == null)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token });
                    
                }
                else
                {
                    return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng");
                }    

                // Nếu đăng nhập thành công, tạo và trả về JWT token
               
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ nếu cần
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }
        private string GenerateJwtToken(NguoiiDung user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    // Các thông tin khác của người dùng có thể được thêm vào các Claim ở đây
                }),
                Expires = DateTime.UtcNow.AddMinutes(1), // Thời gian hết hạn của token (ví dụ: 1 phút)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
