using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce.Contract.OrderContract
{
    public class OrderRequest
    {
        [Required]
        public decimal TotoalPrice { get; set; }

        public DateTime? ShippedDate { get; set; }
        public DateTime? ArriveDate { get; set; }


    }
}