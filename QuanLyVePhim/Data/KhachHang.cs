using System.ComponentModel.DataAnnotations;

namespace QuanLyVePhim.Data
{
    public class KhachHang
    {
        [Key]
        public string? MaKhachhang { get; set; }
        [Required]
        public string? HoTen {  get; set; }
        public string? DiaChi {  get; set; }
        public string? SDT { get; set; }
        public DateTime NgaySinh { get; set; }

    }
}
