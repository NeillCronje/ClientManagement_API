using ClientManagementAPI.Data;
using ClientManagementAPI.Models;
using ClientManagementAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClientManagementAPI.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly ApiContext _db;
        public ClientRepository(ApiContext db): base(db)
        {
            _db = db;

            //var clients = new List<Client>
            //{
            //    new Client
            //    {
            //        Id = 1,
            //        Address = "1 Blue Street",
            //        Age = 32,
            //        City = "Pretoria",
            //        CreatedDate = DateTime.Now,
            //        FirstName = "Neill",
            //        LastName = "Cronje",
            //        Region = "Gauteng",
            //        PostalCode = "0044",
            //        Email = "neillcronje@gmail.com",
            //    },
            //    new Client
            //    {
            //        Id = 2,
            //        Address = "123 Red Street",
            //        Age = 30,
            //        City = "Cape Town",
            //        CreatedDate = DateTime.Now,
            //        FirstName = "Gideon",
            //        LastName = "Dewd",
            //        Region = "Western Cape",
            //        PostalCode = "5521",
            //        Email = "randomdude04@gmail.com",
            //    },
            //    new Client
            //    {
            //        Id = 3,
            //        Address = "7 Law Street",
            //        Age = 44,
            //        City = "Johannesburg",
            //        CreatedDate = DateTime.Now,
            //        FirstName = "Ivan",
            //        LastName = "Smith",
            //        Region = "Gauteng",
            //        PostalCode = "1223",
            //        Email = "hippylover52@gmail.com",
            //    },
            //};

            //_db.Clients.AddRange(clients);
            //_db.SaveChanges();
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            client.ModifiedDate = DateTime.Now;
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();

            return client;
        }
    }
}
