using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MYAcademy_IdentityEmail.Entities;

namespace MYAcademy_IdentityEmail.Context
{
    // Burda appuser kullanmamızın sebebi appuser içindeki verilerin de oluşturulması 
    public class EmailContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer("Server=Talha\\SQLEXPRESS;Initial Catalog=IdentityEmailDbNight;Integrated Security=True;TrustServerCertificate=True;");
        }

        public DbSet<Category> Categories { get; set; }
         public DbSet<Message> Messages { get; set; }
    }
}