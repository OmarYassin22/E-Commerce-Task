using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstNme { get; set; }
        public string LastNme { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public IList<Order> Orders { get; set; }
    }
}