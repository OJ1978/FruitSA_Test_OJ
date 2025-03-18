using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Products> Products => Set<Products>(); //Products Table
        public DbSet<Categories> Categories => Set<Categories>(); //Categories Table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //Create ViewModels for DB
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false); //Saving with Cancelling Enabled
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult(); //Save to DB
        }
    }
}
