using System.ComponentModel.DataAnnotations;

namespace QuanLyVePhim.Data
{
    public class NhanVien
    {
        [Key]
        public string? MaNhanVien {  get; set; }
        [Required]
        public string? HoTen { get; set; }   
        public string? DiaCHi { get; set; }
        public string? SDT { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
    }
}
