using Microsoft.EntityFrameworkCore;
using server.Models;
namespace server.DAL
{
    public class PDbContext : DbContext
    {
        public PDbContext(DbContextOptions<PDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<DonorGift> DonorGift { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GiftCategory> giftCategories { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Status> Statuse { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Donor>()
            .HasMany(d => d.DonorGifts)
            .WithOne(dg => dg.Donor)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>()
            .HasMany(d => d.GiftCategories)
            .WithOne(dg => dg.Category)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Gift>()
            .HasMany(d => d.DonorGifts)
            .WithOne(dg => dg.Gift)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Gift>()
            .HasMany(d => d.GiftCategories)
            .WithOne(dg => dg.Gift)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
            .HasMany(d => d.Tickets)
            .WithOne(dg => dg.User)
            .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Gift>()
            //.HasOne(g => g.Winner)
            //.WithMany()
            //.HasForeignKey(g => g.UserWinnerId)
            //.OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Gift)
            .WithMany(g => g.Tickets)
            .HasForeignKey(t => t.GiftId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
