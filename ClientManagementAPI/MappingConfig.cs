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

            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();

            CreateMap<Client, ClientCreateDTO>().ReverseMap();
            CreateMap<Client, ClientUpdateDTO>().ReverseMap();

            CreateMap<Company, CompanyCreateDTO>().ReverseMap();
            CreateMap<Company, CompanyUpdateDTO>().ReverseMap();
        }
    }
}
