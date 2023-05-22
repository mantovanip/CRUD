using Microsoft.EntityFrameworkCore;

namespace crud.Models
{
    public class BancoDeDados : DbContext
    {
        public DbSet<Clientes> Clientes { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=crud;Username=postgres;Password=123");
        }
    }
}
