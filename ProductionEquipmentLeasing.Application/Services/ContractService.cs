using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Exceptions;
using ProductionEquipmentLeasing.Application.Interfaces;
using ProductionEquipmentLeasing.Application.Interfaces.Services;
using System.Diagnostics.Contracts;

namespace ProductionEquipmentLeasing.Application.Services;

public class ContractService : IContractService
{
    private readonly IRepository<PlacementContract> _contractRepo;
    private readonly IRepository<ProductionFacility> _facilityRepo;
    private readonly IRepository<EquipmentType> _equipmentRepo;
    private readonly IMapper _mapper;
    private readonly IServiceBusSenderService _serviceBus;
    public ContractService(
        IRepository<PlacementContract> contractRepo,
        IRepository<ProductionFacility> facilityRepo,
        IRepository<EquipmentType> equipmentRepo,
        IMapper mapper,
        IServiceBusSenderService serviceBus)
    {
        _contractRepo = contractRepo;
        _facilityRepo = facilityRepo;
        _equipmentRepo = equipmentRepo;
        _mapper = mapper;
        _serviceBus = serviceBus;
    }

    public async Task<Guid> CreateAsync(CreateContractDto dto)
    {
        var facility = await _facilityRepo.Query()
            .Include(c => c.Contracts)
            .Where(x => x.Code == dto.FacilityCode)
            .FirstOrDefaultAsync();
        if (facility == null)
            throw new NotFoundException(nameof(ProductionFacility), dto.FacilityCode);

        var equipmentType = await _equipmentRepo.GetFirstByExpressionAsync(x => x.Code == dto.EquipmentTypeCode);
        if (equipmentType == null)
            throw new NotFoundException(nameof(EquipmentType), dto.EquipmentTypeCode);

        ValidateRequiredOpenArea(facility, equipmentType, dto.Quantity);

        var contract = new PlacementContract(facility, equipmentType, dto.Quantity);

        await _contractRepo.AddAsync(contract);

        await _serviceBus.SendContractForProcessingAsync(contract.Id);

        return contract.Id;
    }

    public async Task<decimal> CalculateRequiredArea(Guid equipmentTypeId, int quantity)
    {
        var equipment = await _equipmentRepo.GetByIdAsync(equipmentTypeId);
        if (equipment == null) throw new NotFoundException(nameof(EquipmentType), equipmentTypeId);

        return equipment.Area * quantity;
    }

    public async Task<ContractDto> GetByIdAsync(Guid id)
    {
        var contract = await _contractRepo.Query()
            .Include(c => c.ProductionFacility)
            .Include(c => c.EquipmentType)
            .FirstOrDefaultAsync(c => c.Id == id);

        return contract == null
            ? throw new NotFoundException(nameof(Contract), id)
            : _mapper.Map<ContractDto>(contract);
    }

    public async Task<IEnumerable<ContractDto>> GetAllAsync()
    {
        return await _contractRepo.Query()
            .Include(c => c.ProductionFacility)
            .Include(c => c.EquipmentType)
            .Select(c => _mapper.Map<ContractDto>(c))
            .ToListAsync();
    }

    public async Task UpdateAsync(UpdateContractDto dto)
    {
        var contract = await _contractRepo.Query()
            .FirstOrDefaultAsync(c => c.Id == dto.Id);

        if (contract == null)
            throw new NotFoundException(nameof(Contract), dto.Id);

        if (dto.Quantity > contract.EquipmentQuantity)
        {
            decimal equipmentArea = await _equipmentRepo.Query()
                .Where(e => e.Id == contract.EquipmentTypeId)
                .Select(e => e.Area)
                .FirstOrDefaultAsync();

            decimal requiredArea = equipmentArea * dto.Quantity;
            decimal availableArea = await _facilityRepo.Query()
                .Where(f => f.Id == contract.ProductionFacilityId)
                .Select(f => f.Area)
                .FirstOrDefaultAsync();

            if (requiredArea > availableArea)
                throw new DomainException("New quantity exceeds facility capacity");
        }

        _mapper.Map(dto, contract);
        await _contractRepo.UpdateAsync(contract);
    }

    public async Task DeleteAsync(Guid id)
    {
        var contract = await _contractRepo.GetByIdAsync(id);
        if (contract == null) return;

        await _contractRepo.DeleteAsync(contract);
    }

    private void ValidateRequiredOpenArea(ProductionFacility facility, EquipmentType equipmentType, int quantity)
    {
        decimal requiredArea = equipmentType.Area * quantity;

        decimal availableArea = facility.Area;

        decimal reservedArea = facility.Contracts.Sum(x => x.EquipmentQuantity * x.EquipmentType.Area);

        if (requiredArea + reservedArea > availableArea)
        {
            throw new DomainException(
                $"Required area ({requiredArea}) exceeds facility free capacity of ({availableArea - reservedArea})");
        }
    }
}
