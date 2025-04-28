using AutoMapper;
using Client.Models;
using Server.Shared.DTOs;

namespace Client
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SupplierDto, Supplier>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<WarehouseDto, Warehouse>().ReverseMap();
            CreateMap<MeasurementUnitDto, MeasurementUnit>().ReverseMap();
            CreateMap<MaterialItemDto, MaterialItem>().ReverseMap();
            CreateMap<MaterialMovementDto, MaterialMovement>().ReverseMap();
        }
    }
}
