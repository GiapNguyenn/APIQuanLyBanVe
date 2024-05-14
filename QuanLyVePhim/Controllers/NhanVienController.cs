using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVePhim.Data;
using System.Security.Cryptography;

namespace QuanLyVePhim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public NhanVienController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetNhanVienByID(string Manhanvien)
        {
            List<NhanVien>? ListNhanVien = null;
            try
            {
                ListNhanVien = await _context.Set<NhanVien>().FromSqlInterpolated($"Exec LTA_GetNhanVienbyId {Manhanvien.Trim().ToLower()}").ToListAsync();
                if (ListNhanVien == null)
                {
                    NotFound("Ko tim thay nhan vien Ma nhan vien");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(ListNhanVien);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNahnVien(NhanVien entity)
        {
            try
            {
                List<NhanVien>? nhanvienlistEntity = null;
                nhanvienlistEntity = await _context.Set<NhanVien>().FromSqlInterpolated($"Exec LTA_GetNhanVienbyId {entity.MaNhanVien.Trim().ToLower()}").ToListAsync();
                if (nhanvienlistEntity == null)
                {
                    return NotFound("Khong tim thay ma nha vien");
                }
                nhanvienlistEntity[0].MaNhanVien=entity.MaNhanVien;
                nhanvienlistEntity[0].HoTen = entity.HoTen;
                nhanvienlistEntity[0].DiaCHi = entity.DiaCHi;
                nhanvienlistEntity[0].SDT = entity.SDT;
                nhanvienlistEntity[0].NgaySinh = entity.NgaySinh;
                nhanvienlistEntity[0].GioiTinh = entity.GioiTinh;
                await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC LTA_UpdateNhanVien {nhanvienlistEntity[0].MaNhanVien},{nhanvienlistEntity[0].HoTen},{nhanvienlistEntity[0].DiaCHi},{nhanvienlistEntity[0].SDT},{nhanvienlistEntity[0].NgaySinh.ToString("yyyy-MM-dd")},{nhanvienlistEntity[0].GioiTinh}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok("Cap nhat thanh cong");

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteByMaNhanVien(string maNhanVien)
        {
            List<NhanVien> existingEntity = await _context.Set<NhanVien>()
                             .FromSqlInterpolated($"LTA_GetNhanVienbyId {maNhanVien}")
                             .ToListAsync();

            if (existingEntity == null)
            {
                return NotFound();
            }

            await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC LTA_DeleteByMaNhanVien {maNhanVien}");

            return Ok("Đã Xóa Thành Công ");
        }

    }

}




