using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVePhim.Data;

namespace QuanLyVePhim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiVeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LoaiVeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDonGia([FromBody] UpdateDonGiaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC LTA_UpdateDonGiaTheoTenLoaiVe 
                    @Title = {request.Title}, 
                    @TenLoaiVe = {request.TenLoaiVe}");
                return Ok(new { Message = "Cập nhật đơn giá thành công." });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
           
        }

    }
}
