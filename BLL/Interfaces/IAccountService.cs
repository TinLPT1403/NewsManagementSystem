using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task<string?> AuthenticateAsync(string email, string password);
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        Task<SystemAccount> GetAccountByIdAsync(int id);
        Task CreateAccountAsync(SystemAccount account);
        Task UpdateAccountAsync(int id, SystemAccount account);
        Task DeleteAccountAsync(int id);
    }
}
