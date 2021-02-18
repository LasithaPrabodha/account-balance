using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Account, AccountDto>();
            CreateMap<Account, AccountWithDetailsDto>();

            CreateMap<Transaction, TransactionDto>();
            CreateMap<Transaction, TransactionWithDetailsDto>();

            CreateMap<UserForRegistrationDto, User>()
               .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

        }
    }
}
