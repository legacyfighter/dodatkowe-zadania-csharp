using System;
using System.Threading.Tasks;
using LegacyFighter.Dietary.Models;
using LegacyFighter.Dietary.Models.NewProducts;
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
        public DbSet<OldProduct> OldProducts { get; set; }
        public DbSet<OldProductDescription> OldProductDescriptions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<TaxConfig> TaxConfigs { get; set; }
        public DbSet<TaxRule> TaxRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.Group)
                    .WithOne(x => x.Customer);
            });
            modelBuilder.Entity<CustomerOrderGroup>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.Customer)
                    .WithOne(x => x.Group)
                    .HasForeignKey<CustomerOrderGroup>(x => x.CustomerId);
                builder.HasOne(x => x.Parent);
                builder.HasMany(x => x.Children);
                builder.HasMany(x => x.Orders);
            });

            modelBuilder.Entity<OldProduct>(builder =>
            {
                builder.HasKey(x => x.Id);
            });

            modelBuilder.Entity<OrderLine>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.Order);
                builder.HasOne(x => x.Product);
            });

            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.CustomerOrderGroup);
                builder.HasMany(x => x.Items);
                builder.HasMany(x => x.TaxRules);
            });
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(x => x.Id);
            });
            modelBuilder.Entity<TaxRule>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.TaxConfig);
            });
            modelBuilder.Entity<TaxConfig>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasMany(x => x.TaxRules);
                builder.Property(x => x.CountryCode)
                    .HasConversion(x => x.AsString(), x => new CountryCode(x));
            });
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
