using ProductionEquipmentLeasing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Application.Interfaces.Services;
public interface IContractService
{
    Task<ContractDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ContractDto>> GetAllAsync();
    Task<Guid> CreateAsync(CreateContractDto dto);
    Task UpdateAsync(UpdateContractDto dto);
    Task DeleteAsync(Guid id);
    Task<decimal> CalculateRequiredArea(Guid equipmentTypeId, int quantity);
}
