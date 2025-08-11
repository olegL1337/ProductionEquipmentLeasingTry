using ProductionEquipmentLeasing.Application.DTOs;

namespace ProductionEquipmentLeasing.Application.Interfaces.Services;

public interface IProductionFacilityService
{
    Task<ProductionFacilityDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductionFacilityDto>> GetAllAsync();
    Task<Guid> CreateAsync(CreateProductionFacilityDto dto);
    Task UpdateAsync(UpdateProductionFacilityDto dto);
    Task DeleteAsync(Guid id);
    Task<bool> CodeExistsAsync(string code);
}
