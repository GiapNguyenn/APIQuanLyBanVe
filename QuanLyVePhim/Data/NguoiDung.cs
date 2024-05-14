using System.ComponentModel.DataAnnotations;

namespace QuanLyVePhim.Data
{
    public class NguoiDung
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? userName { get; set; }
        [Required]
        public string? Password { get; set; }   
        public string? Email {  get; set; } 
        
    }
}
