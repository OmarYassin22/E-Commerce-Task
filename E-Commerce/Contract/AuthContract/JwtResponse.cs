using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Contract.AuthContract
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}