﻿using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce.Contract
{
    public class CustomerRequest
    {
        [Required]
        [MinLength(4)]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public SysRoles Role { get; set; } = SysRoles.custoemr;

        public string Phone { get; set; }
        public IList<OrderResponse> Orders { get; set; }
    }
    public enum SysRoles
    {
        custoemr,
        admin
    };
}