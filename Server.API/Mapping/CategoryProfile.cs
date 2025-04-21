using AutoMapper;
using Server.Data.Entities;
using Server.Shared.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.API.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
