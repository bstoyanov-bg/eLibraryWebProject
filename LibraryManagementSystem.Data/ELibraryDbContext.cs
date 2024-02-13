using LibraryManagementSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    /// <summary>
    /// The ELibraryDbContext class represents the database context of the application.
    /// </summary>

    public class ELibraryDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ELibraryDbContext(DbContextOptions<ELibraryDbContext> options)
            : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; } = null!;

        public DbSet<Book> Books { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Edition> Editions { get; set; } = null!;

        public DbSet<LendedBook> LendedBooks { get; set; } = null!;

        public DbSet<Rating> Ratings { get; set; } = null!;

        public DbSet<BookCategory> BooksCategories { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BookCategory>(entity =>
            {
                entity
                .HasKey(bc => new { bc.BookId, bc.CategoryId });
            });

            builder.Entity<Rating>(entity =>
            {
                entity
                .HasKey(r => new { r.BookId, r.UserId});
            });

            builder.Entity<LendedBook>(entity =>
            {
                entity
                .HasKey(lb => new { lb.BookId, lb.UserId });
            });

            builder.Entity<Rating>()
                .Property(r => r.BookRating)
                .HasPrecision(18, 2);

            //builder.Entity<LendedBook>()
            //    .HasOne(h => h.)
            //    .WithMany(h => h.)


            // TODO Seed DATA like admin and other stuff

            base.OnModelCreating(builder);
        }
    }
}
