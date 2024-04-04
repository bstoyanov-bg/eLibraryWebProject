using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{

    /// <summary>
    /// This configuration class is used to configure and seed the database with the default roles.
    /// The default roles are Admin and User.
    /// </summary>

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .Property(b => b.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder
                .Property(au => au.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
