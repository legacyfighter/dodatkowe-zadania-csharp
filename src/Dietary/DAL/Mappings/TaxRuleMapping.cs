using LegacyFighter.Dietary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LegacyFighter.Dietary.DAL.Mappings
{
    public class TaxRuleMapping : IEntityTypeConfiguration<TaxRule>
    {
        public void Configure(EntityTypeBuilder<TaxRule> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.TaxConfig);
        }
    }
}