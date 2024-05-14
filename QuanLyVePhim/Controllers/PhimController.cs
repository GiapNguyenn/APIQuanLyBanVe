using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLyVePhim.Data;

namespace QuanLyVePhim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhimController : ControllerBase
    {
        private readonly ApplicationDbContext _phim;
        public PhimController(ApplicationDbContext phim)
        {
            _phim = phim;
        }
        [HttpGet("ALLAndById")]
        public async Task<IActionResult> LayTheoIdVaLayAll(string? maPhim=null)
        {
            List<Phim>? listPhim = null;
            try
            {
                listPhim = await _phim.Set<Phim>().FromSqlInterpolated($"Exec TNG_GetallandGetId {maPhim} ").ToListAsync();
                if(listPhim==null)
                {
                    return NotFound();
                }    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(listPhim);
        }
        [HttpPost("InsertOrUpdate")]
        public async Task<IActionResult> InsertPhim(string maPhim, string maLoaiPhim, string maDangPhim, string tenPhim,string nhaSanXuat)
        {
            try
            {
                await _phim.Database.ExecuteSqlInterpolatedAsync($"Exec TNG_InsertAndUpdatePhim {maPhim},{maLoaiPhim},{maDangPhim},{tenPhim},{nhaSanXuat}");
                return Ok("Thêm bộ phim mới thành công hoặc cập nhật thành công");
            }
            catch (Exception ex)
            {
                return BadRequest("Đã xảy ra lỗi: "+ex.Message);
            }
        }
        [HttpPut("InsertOrUpdate")]
        public async Task<IActionResult> Update([FromBody] Phim phim, string? maPhim = null)
        {
            try
            {
                List<Phim> ListPhimbyId = await _phim.Set<Phim>().FromSqlInterpolated($"Exec TNG_GetallandGetId {maPhim}").ToListAsync();
                if(ListPhimbyId==null)
                {
                    return NotFound();
                }
                ListPhimbyId[0].MaPhim = phim.MaPhim;
                ListPhimbyId[0].MaLoaiPhim = phim.MaLoaiPhim;
                ListPhimbyId[0].MaDangPhim = phim.MaDangPhim;
                ListPhimbyId[0].TenPhim = phim.TenPhim;
                ListPhimbyId[0].NhaSanXuat = phim.NhaSanXuat;
                await _phim.Database.ExecuteSqlInterpolatedAsync($"Exec TNG_InsertAndUpdatePhim {ListPhimbyId[0].MaPhim},{ListPhimbyId[0].MaLoaiPhim},{ListPhimbyId[0].MaDangPhim},{ListPhimbyId[0].TenPhim},{ListPhimbyId[0].NhaSanXuat}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok("Đã cập nhật thành công hoặc cập nhật thành công");
        }
        
    }
}
