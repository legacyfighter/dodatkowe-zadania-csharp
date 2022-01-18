using LegacyFighter.Dietary.Models.NewProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LegacyFighter.Dietary.DAL.Mappings
{
    public class OldProductDescriptionMapping : IEntityTypeConfiguration<OldProductDescription>
    {
        public void Configure(EntityTypeBuilder<OldProductDescription> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}