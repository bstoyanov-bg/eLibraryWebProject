using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{
    /// <summary>
    /// This configuration class is used for custom additions to the fluent api of the Rating entity.
    /// </summary> 

    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder
                .HasKey(r => new { r.BookId, r.UserId });

            builder
                .Property(r => r.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(r => r.Book)
                .WithMany(b => b.Ratings)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(r => r.BookRating)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
