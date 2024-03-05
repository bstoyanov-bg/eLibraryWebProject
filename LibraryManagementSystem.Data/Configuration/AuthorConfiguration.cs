using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder
                .Property(a => a.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder
                .Property(a => a.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
