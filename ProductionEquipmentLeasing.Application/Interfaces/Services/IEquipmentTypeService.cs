using ProductionEquipmentLeasing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Application.Interfaces.Services;
public interface IEquipmentTypeService
{
    Task<EquipmentTypeDto> GetByIdAsync(Guid id);
    Task<IEnumerable<EquipmentTypeDto>> GetAllAsync();
    Task<Guid> CreateAsync(CreateEquipmentTypeDto dto);
    Task UpdateAsync(UpdateEquipmentTypeDto dto);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
