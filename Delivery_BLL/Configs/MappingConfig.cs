using AutoMapper;
using Delivery_DAL.Dto;
using Delivery_DAL.Entity;

namespace Delivery_BLL.Configs
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserRegisterModel>().ReverseMap();
        }
    }
}
