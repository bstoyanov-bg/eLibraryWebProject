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
                .Property(au => au.IsDeleted)
                .HasDefaultValue(false);
        }
    }

    //public class ApplicationUserConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    //{
    //    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    //    {
    //        builder.HasData(
    //            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = AdminRole, NormalizedName = AdminRole.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
    //            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = UserRole, NormalizedName = UserRole.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() }
    //        );
    //    }
    //}
}
