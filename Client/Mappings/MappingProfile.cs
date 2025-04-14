using AutoMapper;
using Client.Models;
using Server.Shared.DTOs;

namespace Client.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SupplierDto, Supplier>().ReverseMap();
        }
    }
}
