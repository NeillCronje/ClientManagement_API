using ClientManagementAPI.Models.Dto;

namespace ClientManagementAPI.Data
{
    public static class ClientStore
    {
        public static List<ClientDTO> clientList = new List<ClientDTO>
            {
                new ClientDTO
                {
                    Id = 1,
                    FirstName = "Neill",
                    LastName = "Cronje",
                    Address = "15 Whipstick Cresc",
                    Email = "neillcronje@gmail.com",
                    Age = 32,
                    City = "Pretoria",
                    Region = "Moreletapark",
                    PostalCode = "0044"
                },
                new ClientDTO
                {
                    Id = 2,
                    FirstName = "Angelique",
                    MiddleName = "Christine",
                    LastName = "Cronje",
                    Address = "15 Whipstick Cresc",
                    Email = "roberts.angelique21@ymail.com",
                    Age = 29,
                    City = "Pretoria",
                    Region = "Moreletapark",
                    PostalCode = "0044"
                }
            };
    }
}
