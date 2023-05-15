using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;

namespace ClientManagementAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<User> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
