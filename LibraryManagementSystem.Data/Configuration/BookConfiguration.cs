using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{
    /// <summary>
    /// This configuration class is used for custom additions to the fluent api of the Book entity.
    /// </summary> 

    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .HasOne(h => h.Author)
                .WithMany(c => c.Books)
                .HasForeignKey(h => h.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
