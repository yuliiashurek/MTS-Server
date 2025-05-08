using AutoMapper;
using Server.Data.Entities;
using Server.Shared.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.API.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Organization, OrganizationInfoDto>().ReverseMap();
        }
    }
}
