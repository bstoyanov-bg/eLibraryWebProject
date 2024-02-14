using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{
    /// <summary>
    /// This configuration class is used for custom additions to the fluent api of the LendedBooks entity.
    /// </summary> 

    public class LendedBooksConfiguration : IEntityTypeConfiguration<LendedBook>
    {
        public void Configure(EntityTypeBuilder<LendedBook> builder)
        {
            builder.HasKey(lb => lb.Id);

            builder
                .HasOne(h => h.Book)
                .WithMany(c => c.LendedBooks)
                .HasForeignKey(h => h.BookId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(h => h.User)
                .WithMany(c => c.LendedBooks)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
