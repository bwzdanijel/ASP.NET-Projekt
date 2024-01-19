using Microsoft.EntityFrameworkCore;

namespace ProjektArbeitDanijelMademidda.Models
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.UserIpAddress)
                .HasMaxLength(15); // Maximale Länge für eine IP-Adresse (z. B. "255.255.255.255")

            base.OnModelCreating(modelBuilder);
        }
    }
}
