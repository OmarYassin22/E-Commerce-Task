using E_Commerce.Contract;
using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using E_Commerce.Contract.OrderContract;
using Mapster;
using NLog;
using System.Linq;

[JwtAuthorize]
[RoutePrefix("api/orders")]
public class OrderController : ApiController
{
    private readonly IOrderRepo _orderRepo;
    private readonly ILogger _logger; 
    public OrderController(IOrderRepo orderRepo)
    {
        _orderRepo = orderRepo;
        _logger = LogManager.GetCurrentClassLogger();  
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("")]
    public async Task<IHttpActionResult> GetAllOrders()
    {
        _logger.Info("Fetching all orders...");

        var orders = await _orderRepo.GetAllAsync();
        _logger.Info($"Retrieved {orders.Count()} orders.");

        return Ok(orders.Adapt<IEnumerable<OrderResponse>>());
    }

    [HttpGet]
    [Route("{orderId}")]
    public async Task<IHttpActionResult> GetOrderById(int orderId)
    {
        _logger.Info($"Fetching order with ID: {orderId}");

        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null)
        {
            _logger.Warn($"Order with ID {orderId} not found.");
            return NotFound();
        }

        _logger.Info($"Order {orderId} retrieved successfully.");
        return Ok(order.Adapt<OrderResponse>());
    }

    [Authorize(Roles = "Customer")]
    [HttpGet]
    [Route("my-orders")]
    public async Task<IHttpActionResult> GetMyOrders(int orderId)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid customerId))
        {
            _logger.Warn("Unauthorized access attempt to 'GetMyOrders'.");
            return Unauthorized();
        }

        _logger.Info($"Fetching orders for Customer ID: {customerId}");
        var orders = await _orderRepo.GetOrdersByCustomerIdAsync(orderId);
        _logger.Info($"{orders.Count()} orders retrieved for customer {customerId}.");

        return Ok(orders);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("")]
    public async Task<IHttpActionResult> CreateOrder(OrderRequest order)
    {
        if (!ModelState.IsValid)
        {
            _logger.Warn("Invalid order request received.");
            return BadRequest(ModelState);
        }

        var dbOrder = order.Adapt<Order>();
        _orderRepo.Add(dbOrder);
        await _orderRepo.SaveAsync();

        _logger.Info($"New order created successfully: Order ID {dbOrder.Id}");
        return Ok(dbOrder.Adapt<OrderResponse>());
    }

    [HttpPut]
    [Route("{orderId}")]
    public async Task<IHttpActionResult> UpdateOrder(int orderId, OrderRequest order)
    {
        if (!ModelState.IsValid)
        {
            _logger.Warn("Invalid order update request.");
            return BadRequest(ModelState);
        }

        var dbOrder = await _orderRepo.GetByIdAsync(orderId);
        if (dbOrder == null)
        {
            _logger.Warn($"Order ID {orderId} not found for update.");
            return NotFound();
        }

        _logger.Info($"Updating order ID {orderId}");
        order.Adapt(dbOrder);
        _orderRepo.Update(dbOrder);
        await _orderRepo.SaveAsync();
        _logger.Info($"Order ID {orderId} updated successfully.");

        return Ok(dbOrder.Adapt<OrderResponse>());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("{orderId}")]
    public async Task<IHttpActionResult> DeleteOrder(int orderId)
    {
        _logger.Info($"Deleting order with ID: {orderId}");

        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null)
        {
            _logger.Warn($"Order ID {orderId} not found for deletion.");
            return NotFound();
        }

        _orderRepo.Delete(order);
        await _orderRepo.SaveAsync();

        _logger.Info($"Order ID {orderId} deleted successfully.");
        return Ok("Order deleted successfully.");
    }
}
