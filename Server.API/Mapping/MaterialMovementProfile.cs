using AutoMapper;
using Server.Data.Entities;
using Server.Shared.DTOs;

namespace Server.API.Mapping
{
    public class MaterialItemProfile : Profile
    {
        public MaterialItemProfile()
        {
            CreateMap<MaterialItem, MaterialItemDto>().ReverseMap();
        }
    }
}
