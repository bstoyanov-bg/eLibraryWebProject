using LibraryManagementSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ELibraryDbContext))
                ?? Assembly.GetExecutingAssembly();

            builder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(builder);
        }
    }
}
