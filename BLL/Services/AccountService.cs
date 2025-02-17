using BLL.Interfaces;
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

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            var account = await _unitOfWork.SystemAccounts.GetByEmailAsync(email);
            if (account == null) return null;

            if (account.AccountPassword != password) return null;

            var role = account.AccountRole switch
            {
                1 => "Admin",
                2 => "Staff",
                3 => "Lecturer",
                _ => string.Empty
            };
            if (string.IsNullOrEmpty(role)) return null;

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()) // Add user ID claim
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<List<SystemAccount>> GetAllAccountsAsync()
        {
            return await _unitOfWork.SystemAccounts.GetAllAsync();
        }

        public async Task<SystemAccount> GetAccountByIdAsync(int id)
        {
            return await _unitOfWork.SystemAccounts.GetByIdAsync(id);
        }

        public async Task CreateAccountAsync(SystemAccount account)
        {
            await _unitOfWork.SystemAccounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(int id, SystemAccount account)
        {
            var existingAccount = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            if (existingAccount == null) throw new KeyNotFoundException("Account not found.");

            existingAccount.AccountName = account.AccountName;
            existingAccount.AccountEmail = account.AccountEmail;
            existingAccount.AccountRole = account.AccountRole;
            existingAccount.AccountPassword = account.AccountPassword;

            await _unitOfWork.SystemAccounts.UpdateAsync(existingAccount);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            if (account == null) throw new KeyNotFoundException("Account not found.");

            await _unitOfWork.SystemAccounts.DeleteAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
