using Domain.Entities.Base;
using System;

namespace Domain.Entities;

public class PlacementContract : EntityBase
{
    public Guid ProductionFacilityId { get; private set; }
    public ProductionFacility ProductionFacility { get; private set; }

    public Guid EquipmentTypeId { get; private set; }
    public EquipmentType EquipmentType { get; private set; }

    public int EquipmentQuantity { get; private set; }

    private PlacementContract() { }

    public PlacementContract(
    Guid facilityId,
    Guid equipmentTypeId,
    int quantity)
    {
        ProductionFacilityId = facilityId;
        EquipmentTypeId = equipmentTypeId;
        EquipmentQuantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive");
    }

    public PlacementContract(
    ProductionFacility facility,
    EquipmentType equipmentType,
    int quantity)
    {
        ProductionFacility = facility;
        EquipmentType = equipmentType;
        EquipmentQuantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive");
    }
}