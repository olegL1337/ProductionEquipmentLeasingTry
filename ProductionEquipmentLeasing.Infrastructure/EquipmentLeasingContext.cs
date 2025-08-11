using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Infrastructure;
public class EquipmentLeasingContext : DbContext
{
    public DbSet<ProductionFacility> Facilities { get; set; }
    public DbSet<EquipmentType> EquipmentTypes { get; set; }
    public DbSet<PlacementContract> Contracts { get; set; }

    public EquipmentLeasingContext(DbContextOptions<EquipmentLeasingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EquipmentLeasingContext).Assembly);
    }
}
