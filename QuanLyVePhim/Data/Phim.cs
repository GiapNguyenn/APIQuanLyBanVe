using System.ComponentModel.DataAnnotations;

namespace QuanLyVePhim.Data
{
    public class Phim
    {
        [Key]
        public string? MaPhim { get; set; }
        public string? MaLoaiPhim { get; set; }
        public string? MaDangPhim { get; set; }
        public string? TenPhim { get; set; }
        public string? NhaSanXuat { get; set; }
    }
}
