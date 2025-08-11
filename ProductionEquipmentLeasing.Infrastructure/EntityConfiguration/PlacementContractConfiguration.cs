using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductionEquipmentLeasing.Domain.Entities.Configurations;

public class PlacementContractConfiguration : IEntityTypeConfiguration<PlacementContract>
{
    public void Configure(EntityTypeBuilder<PlacementContract> builder)
    {
        builder.HasKey(e => e.Id);
    }
}
