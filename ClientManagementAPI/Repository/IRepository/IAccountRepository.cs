using ClientManagementAPI.Models;
using System.Linq.Expressions;

namespace ClientManagementAPI.Repository.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<Account> UpdateAccountAsync(Account client);
    }
}
