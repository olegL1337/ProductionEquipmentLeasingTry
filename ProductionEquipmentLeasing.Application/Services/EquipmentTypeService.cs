using AutoMapper;
using Domain.Entities;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Exceptions;
using ProductionEquipmentLeasing.Application.Interfaces;
using ProductionEquipmentLeasing.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Application.Services;
public class EquipmentTypeService : IEquipmentTypeService
{
    private readonly IRepository<EquipmentType> _repository;
    private readonly IMapper _mapper;

    public EquipmentTypeService(
        IRepository<EquipmentType> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EquipmentTypeDto> GetByIdAsync(Guid id)
    {
        var equipmentType = await _repository.GetByIdAsync(id);
        if (equipmentType == null)
            throw new NotFoundException(nameof(EquipmentType), id);

        return _mapper.Map<EquipmentTypeDto>(equipmentType);
    }

    public async Task<IEnumerable<EquipmentTypeDto>> GetAllAsync()
    {
        var equipmentTypes = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<EquipmentTypeDto>>(equipmentTypes);
    }

    public async Task<Guid> CreateAsync(CreateEquipmentTypeDto dto)
    {
        if (await _repository.ExistsAsync(f => f.Code == dto.Code))
            throw new DomainException("Facility code must be unique");

        var equipmentType = new EquipmentType(
            dto.Code,
            dto.Name,
            dto.Area);

        await _repository.AddAsync(equipmentType);
        return equipmentType.Id;
    }

    public async Task UpdateAsync(UpdateEquipmentTypeDto dto)
    {
        var equipmentType = await _repository.GetByIdAsync(dto.Id);
        if (equipmentType == null)
            throw new NotFoundException(nameof(EquipmentType), dto.Id);

        equipmentType.UpdateDetails(dto.Code, dto.Name, dto.Area);
        await _repository.UpdateAsync(equipmentType);
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipmentType = await _repository.GetByIdAsync(id);
        if (equipmentType == null) return;

        await _repository.DeleteAsync(equipmentType);
    }

    public async Task<bool> ExistsAsync(Guid id)
        => await _repository.GetByIdAsync(id) != null;
}