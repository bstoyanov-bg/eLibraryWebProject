using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{
    /// <summary>
    /// This configuration class is used for custom additions to the fluent api of the Edition entity.
    /// </summary> 

    public class EditionConfiguration : IEntityTypeConfiguration<Edition>
    {
        public void Configure(EntityTypeBuilder<Edition> builder)
        {
            builder
                .HasOne(h => h.Book)
                .WithMany(c => c.Editions)
                .HasForeignKey(h => h.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
