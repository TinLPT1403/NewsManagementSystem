using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        private readonly NewsContext newsContext;
        public SystemAccountRepository(NewsContext context) : base(context)
        {
            newsContext = context;
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _newsContext.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<IEnumerable<SystemAccount>> GetAllForManage()
        {
            return await _newsContext.SystemAccounts.Where(a => a.AccountRole != 3).ToListAsync();
        }
    }
}
