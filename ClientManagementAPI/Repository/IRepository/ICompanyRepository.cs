using ClientManagementAPI.Models;
using System.Linq.Expressions;

namespace ClientManagementAPI.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public Task<Company> UpdateCompanyAsync(Company client);
    }
}
