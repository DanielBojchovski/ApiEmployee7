using AutoMapper;
using ApiEmployee7.Models;
namespace ApiEmployee7.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<EmployeeDTO, Employee>().ReverseMap();
        }
    }
}
