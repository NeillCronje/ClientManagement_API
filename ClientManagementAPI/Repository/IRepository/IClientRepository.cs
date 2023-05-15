using ClientManagementAPI.Models;
using System.Linq.Expressions;

namespace ClientManagementAPI.Repository.IRepository
{
    public interface IClientRepository : IRepository<Client>
    {
        public Task<Client> UpdateClientAsync(Client client);
    }
}
