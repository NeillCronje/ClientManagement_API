﻿using ClientManagementAPI.Data;
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
