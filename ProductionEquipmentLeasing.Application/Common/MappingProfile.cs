using AutoMapper;
using Domain.Entities;
using ProductionEquipmentLeasing.Application.DTOs;

namespace ProductionEquipmentLeasing.Application.Common;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EquipmentType, EquipmentTypeDto>();
        CreateMap<CreateEquipmentTypeDto, EquipmentType>();
        CreateMap<UpdateEquipmentTypeDto, EquipmentType>();

        CreateMap<ProductionFacility, ProductionFacilityDto>();
        CreateMap<CreateProductionFacilityDto, ProductionFacility>();
        CreateMap<UpdateProductionFacilityDto, ProductionFacility>();

        CreateMap<PlacementContract, ContractDto>();
        CreateMap<CreateContractDto, PlacementContract>();
        CreateMap<UpdateContractDto, PlacementContract>();
    }
}
