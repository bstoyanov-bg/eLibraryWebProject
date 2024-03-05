using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{
    /// <summary>
    /// This configuration class is used for custom additions to the fluent api of the BookCategory entity.
    /// </summary> 

    public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
    {
        public void Configure(EntityTypeBuilder<BookCategory> builder)
        {
            builder
                .HasKey(bc => new { bc.BookId, bc.CategoryId });
        }
    }
}
