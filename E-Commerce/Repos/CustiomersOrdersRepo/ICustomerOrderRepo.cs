using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Contract
{
    public interface ICustomersOrdersRepo
    {
        Task<IEnumerable<ApplicationUser>> GetCustomersByOrderIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<bool> AssignCustomerToOrderAsync(string customerId, int orderId);
        Task<bool> RemoveCustomerFromOrderAsync(string customerId, int orderId);
    }
}
    