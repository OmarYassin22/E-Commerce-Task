using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Contract
{
    public class OrderRequest
    {

        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public CustomerRequest Customer { get; set; }
    }
}