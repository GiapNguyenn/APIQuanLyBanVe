using System.ComponentModel.DataAnnotations;

namespace QuanLyVePhim.Data
{
    public class NguoiiDung
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]    
        public string Name { get; set; }
        [Required]
        [MaxLength(70)]
        public string Password { get; set; }   
        public string Email {  get; set; }  
        
    }
}
