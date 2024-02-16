using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Data.Configuration
{

    /// <summary>
    /// This configuration class is used to configure and seed the database with the default roles.
    /// The default roles are Admin and User.
    /// </summary>

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = AdminRole, NormalizedName = AdminRole.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = UserRole, NormalizedName = UserRole.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() }
            );
        }
    }
}
