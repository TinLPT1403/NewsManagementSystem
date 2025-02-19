using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Utils
{
    public class UserUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserUtils(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetUserFromToken()
        {

            // Retrieve token from the cookie
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return -1;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "NameIdentifier" || c.Type == "nameid" );
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return -1;
        }

        public int GetUserFromInputToken(string token)
        {

            if (string.IsNullOrEmpty(token))
            {
                return -1;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "NameIdentifier" || c.Type == "nameid");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return -1;
        }

        public string GetRoleFromToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
            Console.WriteLine($"AAAAAAAAAAAAAAAA TOKEN IS: {token}");
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            var roleClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" || c.Type == "Role");

            if (roleClaim != null)
            {
                return roleClaim.Value;
            }
            return null;
        }
    }
}
