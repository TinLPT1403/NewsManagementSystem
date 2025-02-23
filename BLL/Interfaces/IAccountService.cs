using BLL.DTOs;
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
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        Task<IEnumerable<SystemAccount>> GetAllAccountsForManageAsync();
        Task<SystemAccount> GetAccountByIdAsync(int id);
        Task CreateAccountAsync(AccountCreateDTO account);
        Task CreateAccountAsync(AccountCreateAdminDTO account);
        Task UpdateAccountAsync(int id, AccountDTO account);
        Task UpdateAccountAsync(int id, AccountUpdateAdminDTO account);
        Task DeleteAccountAsync(int id);

        
    }
}
