using E_Commerce.Contract.CustomerContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Contract.CustomerOrderContract
{
    public class CustomerOrderResponse
    {

        public string CustomerId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }


    }
}