using AutoMapper;
using VehicleApplication.Model;
using VehicleApplication.WebAPI.Models;

namespace VehicleApplication.WebAPI.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDto>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDto>().ReverseMap();
        }
    }
}
