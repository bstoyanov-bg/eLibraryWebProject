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
                .HasKey(lb => new { lb.BookId, lb.UserId });

            builder
                .HasOne(h => h.Book)
                .WithMany(c => c.Ratings)
                .HasForeignKey(h => h.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(h => h.User)
                .WithMany(c => c.Ratings)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(r => r.BookRating)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
