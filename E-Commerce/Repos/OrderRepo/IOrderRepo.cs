using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Contract
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        Task<IEnumerable<ApplicationUser>> GetOrdersByCustomerIdAsync(int orderId);
    }
}
