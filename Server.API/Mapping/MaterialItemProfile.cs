using AutoMapper;
using Server.Data.Entities;
using Server.Shared.DTOs;

namespace Server.API.Mapping
{
    public class MaterialMovementProfile : Profile
    {
        public MaterialMovementProfile()
        {
            CreateMap<MaterialMovement, MaterialMovementDto>().ReverseMap();
        }
    }
}
