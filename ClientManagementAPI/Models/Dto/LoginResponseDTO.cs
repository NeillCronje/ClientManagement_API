namespace ClientManagementAPI.Models.Dto
{
    public class LoginResponseDTO
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
