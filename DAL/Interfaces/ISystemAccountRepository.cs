using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<IEnumerable<SystemAccount>> GetAllForManage();
    }
}
