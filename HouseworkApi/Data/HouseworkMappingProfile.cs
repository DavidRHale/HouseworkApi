using AutoMapper;
using HouseworkApi.ViewModels;

namespace HouseworkApi.Data
{
  public class HouseworkMappingProfile : Profile 
  {
    public HouseworkMappingProfile()
    {
      CreateMap<Room, RoomViewModel>()
        .ForMember(rvm => rvm.RoomId, ex => ex.MapFrom(r => r.Id))
        .ReverseMap();
    }
  }
}