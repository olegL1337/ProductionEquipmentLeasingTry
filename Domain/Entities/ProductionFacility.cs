using Domain.Entities.Base;
using System;

namespace Domain.Entities;

public class ProductionFacility : EntityBase
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Area { get; set; }

    public ICollection<PlacementContract> Contracts { get; set; } = [];

    private ProductionFacility() { }

    public ProductionFacility(string code, string name, decimal area)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Area = area > 0 ? area : throw new ArgumentException("Area must be positive");
    }

    public void UpdateDetails(string code, string name, decimal area)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Area = area > 0 ? area : throw new ArgumentException("Area must be positive");
    }
}
