using AdventureWorksService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksService.Data
{
    public class AdventureWorksContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        // Доданий конструктор, який приймає DbContextOptions
        public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AdventureWorksLocal;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Встановлення відносин між Client та Ticket
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Tickets)
                .WithOne(t => t.Client)
                .HasForeignKey(t => t.ClientId);

            // Встановлення відносин між Bus та Ticket
            modelBuilder.Entity<Bus>()
                .HasMany(b => b.Tickets)
                .WithOne(t => t.Bus)
                .HasForeignKey(t => t.BusId);
        }
    }


}
