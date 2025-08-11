using Domain.Entities.Base;

namespace Domain.Entities;

public class EquipmentType : EntityBase
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Area { get; set; }

    public EquipmentType(string code, string name, decimal area)
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
