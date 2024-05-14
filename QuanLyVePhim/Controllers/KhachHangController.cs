using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVePhim.Data;

namespace QuanLyVePhim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public KhachHangController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<KhachHang> KhachHanglist=null;
            try
            {
                KhachHanglist = await _context.Set<KhachHang>().FromSqlInterpolated($"EXEC LAK_GETALLKhachHang").ToListAsync();
                if (KhachHanglist == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(KhachHanglist);
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetID(string id)
        {
            List<KhachHang>? KhachHanglist2 = null;
            try
            {
                KhachHanglist2 = await _context.Set<KhachHang>().FromSqlInterpolated($"EXEC LAK_GETIDKhachHang {id}").ToListAsync();
                if (KhachHanglist2 == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
            return Ok(KhachHanglist2);
        }


    }
}
