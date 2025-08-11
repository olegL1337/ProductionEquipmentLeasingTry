using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Application.DTOs;
public record ContractDto(
    Guid Id,
    Guid FacilityId,
    string FacilityName,
    Guid EquipmentTypeId,
    string EquipmentTypeName,
    int Quantity,
    decimal TotalArea)
{
    public ContractDto() : this(default, default, string.Empty, default, string.Empty, default, default)
    {
    }
}

public record CreateContractDto(
    string FacilityCode,
    string EquipmentTypeCode,
    int Quantity)
{
    public CreateContractDto() : this(string.Empty, string.Empty, default)
    {
    }
}

public record UpdateContractDto(
    Guid Id,
    int Quantity)
{
    public UpdateContractDto() : this(default, default)
    {

    }
};

