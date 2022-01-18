using LegacyFighter.Dietary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LegacyFighter.Dietary.DAL.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.CustomerOrderGroup);
            builder.HasMany(x => x.Items);
            builder.HasMany(x => x.TaxRules);
        }
    }
}