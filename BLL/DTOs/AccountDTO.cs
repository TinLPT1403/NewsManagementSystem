using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{

    public class AccountCreateDTO
    {
        public required string AccountName { get; set; }
        public required string AccountEmail { get; set; }
        public required string Password { get; set; }
    }

    public class AccountDTO
    {
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public string? Password { get; set; }
    }

    public class AccountCreateAdminDTO
    {
        public required string AccountName { get; set; }
        public required string AccountEmail { get; set; }
        public required int AccountRole { get; set; }
        public required string Password { get; set; }
    }

    public class AccountUpdateAdminDTO
    {
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
        public string? Password { get; set; }
    }
}
