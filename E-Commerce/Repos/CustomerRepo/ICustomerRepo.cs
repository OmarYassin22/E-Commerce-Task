using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Contract
{
    public interface ICustomerRepo : IGenericRepo<ApplicationUser>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
    }
}
