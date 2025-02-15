using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using E_Commerce.Contract;
using E_Commerce.Models;

public class OrderRepo : GenericRepo<Order>, IOrderRepo
{
    private readonly AppDbContext _context;

    public OrderRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplicationUser>> GetOrdersByCustomerIdAsync(int orderId)
    {
        var order = await _context.Orders.Include(o => o.Customers).FirstOrDefaultAsync(o => o.Id == orderId);
        return order?.Customers ?? new List<ApplicationUser>();
    }
}
