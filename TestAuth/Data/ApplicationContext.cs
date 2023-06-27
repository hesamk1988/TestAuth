using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAuth.Data.Configurations;
using TestAuth.Data.Entities;

namespace TestAuth.Data
{
    public class ApplicationContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.Entity<Person>().OwnsMany(x => x.Addresses, ad =>
            {
                ad.Property(p => p.City).HasMaxLength(30);
                ad.Property(p => p.Street).HasMaxLength(30);
            });
        }

        public DbSet<Person> Persons { get; set; }

    }
}
