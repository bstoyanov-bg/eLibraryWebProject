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
            //builder
            //    .HasOne(g => g.Manager)
            //    .WithMany(m => m.Gyms)
            //    .HasForeignKey(g => g.ManagerId)
            //    .OnDelete(DeleteBehavior.Restrict)
            //    .IsRequired();
        }
    }
}
