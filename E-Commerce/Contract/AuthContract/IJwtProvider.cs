using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Contract.AuthContract
{
    public interface IJwtProvider
    {
        Task<JwtResponse> GenerateTokenAsync(ApplicationUser user);
         //(ClaimsPrincipal Principal, string Error) ValidateToken(string token);

    }
}
        