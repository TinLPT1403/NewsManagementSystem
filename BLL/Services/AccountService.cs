using BLL.DTOs;
using BLL.Interfaces;
using BLL.Utils;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AccountService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            var account = await _unitOfWork.SystemAccounts.GetByEmailAsync(email);
            if (account == null) return null;

            if (!PasswordUtils.VerifyPassword(password, account.AccountPasswordHash.ToString())) return null;



            return account;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _unitOfWork.SystemAccounts.GetAllAsync();
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccountsForManageAsync()
        {
            return await _unitOfWork.SystemAccounts.GetAllForManage();
        }

        public async Task<SystemAccount> GetAccountByIdAsync(int id)
        {
            return await _unitOfWork.SystemAccounts.GetByIdAsync(id);
        }

        public async Task CreateAccountAsync(AccountCreateDTO dto)
        {
            var account = new SystemAccount
            {
                AccountName = dto.AccountName,
                AccountEmail = dto.AccountEmail,
                AccountPasswordHash = PasswordUtils.HashPassword(dto.Password),
                AccountRole = 1
            };
            await _unitOfWork.SystemAccounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CreateAccountAsync(AccountCreateAdminDTO dto)
        {
            var account = new SystemAccount
            {
                AccountName = dto.AccountName,
                AccountEmail = dto.AccountEmail,
                AccountRole = dto.AccountRole,
                AccountPasswordHash = PasswordUtils.HashPassword(dto.Password)
            };

            await _unitOfWork.SystemAccounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(int id, AccountDTO account)
        {
            var existingAccount = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            if (existingAccount == null) throw new KeyNotFoundException("Account not found.");

            if (!string.IsNullOrEmpty(account.AccountName))
            {
                existingAccount.AccountName = account.AccountName;
            }

            if (!string.IsNullOrEmpty(account.AccountEmail))
            {
                existingAccount.AccountEmail = account.AccountEmail;
            }

            if (!string.IsNullOrEmpty(account.Password))
            {
                existingAccount.AccountPasswordHash = PasswordUtils.HashPassword(account.Password);
            }

            await _unitOfWork.SystemAccounts.UpdateAsync(existingAccount);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(int id, AccountUpdateAdminDTO account)
        {
            var existingAccount = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            if (existingAccount == null) throw new KeyNotFoundException("Account not found.");

            if (!string.IsNullOrEmpty(account.AccountName))
            {
                existingAccount.AccountName = account.AccountName;
            }

            if (!string.IsNullOrEmpty(account.AccountEmail))
            {
                existingAccount.AccountEmail = account.AccountEmail;
            }

            if(account.AccountRole.HasValue)
            {
                existingAccount.AccountRole = account.AccountRole.Value;
            }

            if (!string.IsNullOrEmpty(account.Password))
            {
                existingAccount.AccountPasswordHash = PasswordUtils.HashPassword(account.Password);
            }

            await _unitOfWork.SystemAccounts.UpdateAsync(existingAccount);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _unitOfWork.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId == id);
            if (account == null) throw new KeyNotFoundException("Account not found.");

            _unitOfWork.SystemAccounts.Delete(account);
            await _unitOfWork.SaveChangesAsync();
        }

      
    }
}
