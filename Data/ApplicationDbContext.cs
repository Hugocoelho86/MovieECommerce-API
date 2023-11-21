using ECommerceMovies.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMovies.API.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Administrator", NormalizedName = "ADMIN"},
                new IdentityRole { Name = "User", NormalizedName = "USER"}
                );

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Items)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.Items)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.Actors)
                .WithMany(e => e.Movies);
        }
    }
}
