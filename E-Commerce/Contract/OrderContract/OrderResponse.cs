using E_Commerce.Contract.OrderContract;
using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce.Contract
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public float TotoalPrice { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime ArriveDate { get; set; }
        public ICollection<CustomersOrders> CustomersOrders { get; set; }
    }
}