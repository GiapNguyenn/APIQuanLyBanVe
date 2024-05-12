using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyVePhim.Controllers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> op): base(op) 
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseSqlServer("name=ConnectionStrings:Connect");
                base.OnConfiguring(optionsBuilder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"lỗi Application {ex.Message}");
            }
        }
    }
}
