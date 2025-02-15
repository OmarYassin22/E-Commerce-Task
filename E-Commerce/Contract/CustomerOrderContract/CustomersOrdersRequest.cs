using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Contract.CustomerOrderContract
{
    public class CustomersOrdersRequest
    {
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}