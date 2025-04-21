using AutoMapper;
using Server.Data.Entities;
using Server.Shared.DTOs;
namespace Server.API.Mapping
{
    public class MeasurementUnitProfile : Profile
    {
        public MeasurementUnitProfile()
        {
            CreateMap<MeasurementUnit, MeasurementUnitDto>().ReverseMap();
        }
    }
}
