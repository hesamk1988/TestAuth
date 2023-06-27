using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestAuth.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            builder.HasData(new IdentityUser { UserName = "admin", PasswordHash = hasher.HashPassword(null, "123") },
                new IdentityUser { UserName = "user", PasswordHash = hasher.HashPassword(null, "123") });
        }
    }
}
