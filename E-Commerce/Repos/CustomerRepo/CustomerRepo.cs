using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using E_Commerce.Contract;
using E_Commerce.Models;

public class CustomerRepo : GenericRepo<ApplicationUser>, ICustomerRepo
{
    private readonly AppDbContext _context;

    public CustomerRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
    {
        var customer = await _context.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == customerId);
        return customer?.Orders ?? new List<Order>();
    }
}
