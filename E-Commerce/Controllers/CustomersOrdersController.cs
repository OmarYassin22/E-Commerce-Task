using E_Commerce.Contract;
using E_Commerce.Contract.CustomerContract;
using E_Commerce.Contract.CustomerOrderContract;
using E_Commerce.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using System.Linq;

[JwtAuthorize]
[RoutePrefix("api/customers-orders")]
public class CustomersOrdersController : ApiController
{
    private readonly ICustomersOrdersRepo _customersOrdersRepo;
    private readonly ILogger _logger;

    public CustomersOrdersController(ICustomersOrdersRepo customersOrdersRepo)
    {
        _customersOrdersRepo = customersOrdersRepo;
        _logger = LogManager.GetCurrentClassLogger();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("order/{orderId}/customers")]
    public async Task<IHttpActionResult> GetCustomersByOrderId(int orderId)
    {
        try
        {
            _logger.Info($"Fetching customers for order ID {orderId}");
            var customers = await _customersOrdersRepo.GetCustomersByOrderIdAsync(orderId);

            _logger.Info($"Retrieved {customers.Count()} customers for order {orderId}");
            return Ok(customers.Adapt<IEnumerable<CustomerReponse>>());
        }
        catch (Exception ex)
        {
            _logger.Error($"Error fetching customers for order {orderId}: {ex.Message}");
            return BadRequest("Something went wrong while retrieving customers.");
        }
    }

    [Authorize]
    [HttpGet]
    [Route("customer/{customerId}/orders")]
    public async Task<IHttpActionResult> GetOrdersByCustomerId(string customerId)
    {
        try
        {
            _logger.Info($"Fetching orders for customer ID {customerId}");
            var orders = await _customersOrdersRepo.GetOrdersByCustomerIdAsync(customerId);

            _logger.Info($"Retrieved {orders.Count()} orders for customer {customerId}");
            return Ok(orders.Adapt<IEnumerable<OrderResponse>>());
        }
        catch (Exception ex)
        {
            _logger.Error($"Error fetching orders for customer {customerId}: {ex.Message}");
            return BadRequest("Something went wrong while retrieving orders.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("assign")]
    public async Task<IHttpActionResult> AssignCustomerToOrder([FromBody] CustomersOrdersRequest model)
    {
        try
        {
            _logger.Info($"Assigning customer {model.CustomerId} to order {model.OrderId}");
            var success = await _customersOrdersRepo.AssignCustomerToOrderAsync(model.CustomerId, model.OrderId);

            if (!success)
            {
                _logger.Warn($"Failed to assign customer {model.CustomerId} to order {model.OrderId}");
                return BadRequest("Failed to assign customer to order.");
            }

            _logger.Info($"Customer {model.CustomerId} successfully assigned to order {model.OrderId}");
            return Ok(model.Adapt<CustomerOrderResponse>());
        }
        catch (Exception ex)
        {
            _logger.Error($"Error assigning customer {model.CustomerId} to order {model.OrderId}: {ex.Message}");
            return BadRequest("Something went wrong while assigning the customer to the order.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("remove/{customerId}/{orderId}")]
    public async Task<IHttpActionResult> RemoveCustomerFromOrder(string customerId, int orderId)
    {
        try
        {
            _logger.Info($"Removing customer {customerId} from order {orderId}");
            var success = await _customersOrdersRepo.RemoveCustomerFromOrderAsync(customerId, orderId);

            if (!success)
            {
                _logger.Warn($"Failed to remove customer {customerId} from order {orderId}");
                return BadRequest("Failed to remove customer from order.");
            }

            _logger.Info($"Customer {customerId} successfully removed from order {orderId}");
            return Ok("Customer removed from order successfully.");
        }
        catch (Exception ex)
        {
            _logger.Error($"Error removing customer {customerId} from order {orderId}: {ex.Message}");
            return BadRequest("Something went wrong while removing the customer from the order.");
        }
    }
}
