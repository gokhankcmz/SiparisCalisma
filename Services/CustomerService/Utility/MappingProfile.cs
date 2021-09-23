using AutoMapper;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;

namespace CustomerService.Utility
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<Customer,CustomerResponseDto>()
                .ForMember(c => c.Address,
                    opt => opt.MapFrom(x => string.Join(' ', x.Address.AddressLine, x.Address.City, x.Address.Country, x.Address.CityCode)));
            ;
            CreateMap<Customer,CustomerCollectionDto>()
                .ForMember(c => c.Address,
                    opt => opt.MapFrom(x => string.Join(' ', x.Address.AddressLine, x.Address.City, x.Address.Country, x.Address.CityCode)));

            CreateMap<UpdateCustomerDto, Customer>();
        }
    }
}