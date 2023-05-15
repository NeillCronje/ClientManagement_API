using AutoMapper;
using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;

namespace ClientManagementAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Client, ClientDTO>();
            CreateMap<ClientDTO, Client>();

            CreateMap<Account, AccountDTO>();
            CreateMap<AccountDTO, Account>();

            CreateMap<Client, ClientCreateDTO>().ReverseMap();
            CreateMap<Client, ClientUpdateDTO>().ReverseMap();

            CreateMap<Account, AccountCreateDTO>().ReverseMap();
            CreateMap<Account, AccountUpdateDTO>().ReverseMap();
        }
    }
}
