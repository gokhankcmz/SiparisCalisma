using AutoMapper;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;

namespace OrderService.Utility
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(c => c.Address,
                opt => opt.MapFrom(x => string.Join(' ', x.Address.AddressLine, x.Address.City, x.Address.Country, x.Address.CityCode)));
            
            CreateMap<Order, OrderCollectionDto>()
                .ForMember(c => c.Address,
                    opt => opt.MapFrom(x => string.Join(' ', x.Address.AddressLine, x.Address.City, x.Address.Country, x.Address.CityCode)));
            
            CreateMap<UpdateOrderDto, Order>();
        }
    }
}