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
                .Property(e => e.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(e => e.Book)
                .WithMany(b => b.Editions)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(e => e.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
