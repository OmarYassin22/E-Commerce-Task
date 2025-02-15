using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using E_Commerce.Contract;
using E_Commerce.Models;

public class CustomersOrdersRepo : ICustomersOrdersRepo
{
    private readonly AppDbContext _context;

    public CustomersOrdersRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplicationUser>> GetCustomersByOrderIdAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Customers)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return order?.Customers ?? new List<ApplicationUser>();
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
    {
        var customer = await _context.Users
            .Include(u => u.Orders)
            .FirstOrDefaultAsync(u => u.Id == customerId);

        return customer?.Orders ?? new List<Order>();
    }

    public async Task<bool> AssignCustomerToOrderAsync(string customerId, int orderId)
    {
        var customer =  _context.Users.Find(customerId);
        var order = await _context.Orders.FindAsync(orderId);

        if (customer == null || order == null) return false;

        customer.Orders.Add(order);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveCustomerFromOrderAsync(string customerId, int orderId)
    {
        var customer = await _context.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == customerId);
        var order = await _context.Orders.FindAsync(orderId);

        if (customer == null || order == null) return false;

        customer.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}
