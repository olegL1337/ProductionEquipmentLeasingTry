namespace ProductionEquipmentLeasing.Application.DTOs;
public record ProductionFacilityDto(
    Guid Id,
    string Code,
    string Name,
    decimal Area,
    DateTime CreatedAt)
{
    public ProductionFacilityDto() : this(default, string.Empty, string.Empty, default, default)
    {

    }
};

public record CreateProductionFacilityDto(
    string Code,
    string Name,
    decimal Area)
{
    public CreateProductionFacilityDto() : this(string.Empty, string.Empty, default)
    {

    }
};

public record UpdateProductionFacilityDto(
    Guid Id,
    string Code,
    string Name,
    decimal Area)
{
    public UpdateProductionFacilityDto() : this(default, string.Empty, string.Empty, default)
    {

    }
};
