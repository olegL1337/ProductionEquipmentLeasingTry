using AutoMapper;
using Domain.Entities;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Exceptions;
using ProductionEquipmentLeasing.Application.Interfaces;
using ProductionEquipmentLeasing.Application.Interfaces.Services;

namespace ProductionEquipmentLeasing.Application.Services;

public class ProductionFacilityService : IProductionFacilityService
{
    private readonly IRepository<ProductionFacility> _repository;
    private readonly IMapper _mapper;

    public ProductionFacilityService(
        IRepository<ProductionFacility> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductionFacilityDto> GetByIdAsync(Guid id)
    {
        var facility = await _repository.GetByIdAsync(id);
        return facility == null
            ? throw new NotFoundException(nameof(ProductionFacility), id)
            : _mapper.Map<ProductionFacilityDto>(facility);
    }

    public async Task<IEnumerable<ProductionFacilityDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<ProductionFacilityDto>>(
            await _repository.GetAllAsync());

    public async Task<Guid> CreateAsync(CreateProductionFacilityDto dto)
    {
        if (await _repository.ExistsAsync(f => f.Code == dto.Code))
            throw new DomainException("Facility code must be unique");

        var facility = _mapper.Map<ProductionFacility>(dto);
        await _repository.AddAsync(facility);
        return facility.Id;
    }

    public async Task UpdateAsync(UpdateProductionFacilityDto dto)
    {
        var facility = await _repository.GetByIdAsync(dto.Id);
        if (facility == null)
            throw new NotFoundException(nameof(ProductionFacility), dto.Id);

        if (await _repository.ExistsAsync(f =>
            f.Code == dto.Code && f.Id != dto.Id))
            throw new DomainException("Facility code must be unique");

        _mapper.Map(dto, facility);
        await _repository.UpdateAsync(facility);
    }

    public async Task DeleteAsync(Guid id)
    {
        var facility = await _repository.GetByIdAsync(id);
        if (facility == null) return;

        await _repository.DeleteAsync(facility);
    }

    public async Task<bool> CodeExistsAsync(string code)
        => await _repository.ExistsAsync(f => f.Code == code);
}
