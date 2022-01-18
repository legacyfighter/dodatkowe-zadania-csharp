using System;
using System.Reflection;
using System.Threading.Tasks;
using LegacyFighter.Dietary.Models;
using Microsoft.EntityFrameworkCore;

namespace LegacyFighter.Dietary.DAL
{
    public class DietaryDbContext : DbContext
    {
        public DietaryDbContext(DbContextOptions<DietaryDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOrderGroup> CustomerOrderGroups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<TaxConfig> TaxConfigs { get; set; }
        public DbSet<TaxRule> TaxRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
        }
        
        public async Task DeleteAsync(object entity)
        {
            Remove(entity);
            await SaveChangesAsync();
        }

        public async Task UpsertAsync(object entity)
        {
            var entry = Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    await AddAsync(entity);
                    break;
                case EntityState.Modified:
                    Update(entity);
                    break;
                case EntityState.Added:
                    await AddAsync(entity);
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new InvalidOperationException();
            }

            await SaveChangesAsync();
        }
    }
}