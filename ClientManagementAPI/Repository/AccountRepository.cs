using ClientManagementAPI.Data;
using ClientManagementAPI.Models;
using ClientManagementAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClientManagementAPI.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly ApiContext _db;
        public AccountRepository(ApiContext db): base(db)
        {
            _db = db;
        }

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            account.ModifiedDate = DateTime.Now;
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();

            return account;
        }
    }
}
