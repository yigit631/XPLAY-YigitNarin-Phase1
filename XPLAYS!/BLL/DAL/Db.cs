using Microsoft.EntityFrameworkCore;

namespace BLL.DAL
{
    public class Db : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Image> Images { get; set; } // Yeni Image tablosu eklendi

        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Game ve Image arasındaki ilişki tanımlandı
            modelBuilder.Entity<Image>()
                .HasOne(i => i.game)
                .WithMany(g => g.Images)
                .HasForeignKey(i => i.GameId)
                .OnDelete(DeleteBehavior.Cascade); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
