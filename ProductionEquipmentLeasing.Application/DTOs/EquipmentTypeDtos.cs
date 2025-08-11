using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Application.DTOs;
public record EquipmentTypeDto(
    Guid Id,
    string Code,
    string Name,
    decimal Area)
{
    public EquipmentTypeDto() : this(default, string.Empty, string.Empty, default)
    {

    }
};

public record CreateEquipmentTypeDto(
    string Code,
    string Name,
    decimal Area)
{
    public CreateEquipmentTypeDto() : this(string.Empty, string.Empty, default)
    {

    }
};

public record UpdateEquipmentTypeDto(
    Guid Id,
    string Code,
    string Name,
    decimal Area)
{
    public UpdateEquipmentTypeDto() : this(default, string.Empty, string.Empty, default)
    {

    }
};
