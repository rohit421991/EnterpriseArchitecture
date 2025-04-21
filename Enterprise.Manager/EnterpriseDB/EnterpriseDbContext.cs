using Enterprise.Manager.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Manager.EnterpriseDB
{
    public class EnterpriseDbContext : IdentityDbContext<ApplicationUser>
    {
        public EnterpriseDbContext(DbContextOptions<EnterpriseDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
