using E_Commerce.Contract;
using E_Commerce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Mapster;
using E_Commerce.Contract.CustomerContract;
using NLog;
using System.Linq;

[JwtAuthorize]
[RoutePrefix("api/customers")]
public class CustomersController : ApiController
{
    private readonly ICustomerRepo _customerRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;

    public CustomersController(ICustomerRepo customerRepo, UserManager<ApplicationUser> userManager, ILogger logger)
    {
        _customerRepo = customerRepo;
        _userManager = userManager;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("")]
    public async Task<IHttpActionResult> GetAllCustomers()
    {
        try
        {
            _logger.Info("Fetching all customers...");

            var customers = await _customerRepo.GetAllAsync();
            var response = customers.Adapt<IEnumerable<CustomerReponse>>();

            _logger.Info($"Retrieved {customers.Count()} customers.");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error fetching customers: {ex.Message}");
            return BadRequest("Something went wrong while fetching customers.");
        }
    }

    [HttpGet]
    [Route("{customerId}")]
    public async Task<IHttpActionResult> GetCustomerById(string customerId)
    {
        try
        {
            _logger.Info($"Fetching customer with ID: {customerId}");

            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.Warn($"No customer found with ID: {customerId}");
                return NotFound();
            }

            var response = customer.Adapt<CustomerReponse>();
            _logger.Info($"Customer {customerId} retrieved successfully.");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error($" Error fetching customer {customerId}: {ex.Message}");
            return BadRequest($"Something went wrong while retrieving customer {customerId}.");
        }
    }

    [Authorize(Roles = "Customer")]
    [HttpGet]
    [Route("orders")]
    public async Task<IHttpActionResult> GetMyOrders()
    {
        try
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.Warn($"Unauthorized access attempt by {User.Identity.Name}");
                return Unauthorized();
            }

            _logger.Info($"Fetching orders for Customer ID: {userId}");
            var orders = await _customerRepo.GetOrdersByCustomerIdAsync(userId);

            _logger.Info($"{orders.Count()} orders retrieved for customer {userId}");
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.Error($" Error fetching orders for customer {User.Identity.Name}: {ex.Message}");
            return BadRequest($"Something went wrong while retrieving orders for {User.Identity.Name}.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("")]
    public async Task<IHttpActionResult> CreateCustomer(CustomerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.Warn($"Invalid customer creation request for email: {request.Email}");
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.Warn($"Customer creation failed: Email {request.Email} already exists.");
                return BadRequest("User already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                PhoneNumber = request.Phone
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                _logger.Error($"Error creating customer {request.Email}: {string.Join(", ", result.Errors)}");
                return BadRequest(result.Errors.ToString());
            }

            await _userManager.AddToRoleAsync(user.Id, request.Role == SysRoles.custoemr ? "Customer" : "Admin");

            var response = request.Adapt<CustomerReponse>();
            response.Id = user.Id;

            _logger.Info($"Customer {request.Email} created successfully.");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error creating customer {request.Email}: {ex.Message}");
            return BadRequest("Something went wrong while creating a new customer.");
        }
    }

    [HttpPut]
    [Route("{customerId}")]
    public async Task<IHttpActionResult> UpdateCustomer(string customerId, CustomerRequest customer)
    {
        try
        {
            _logger.Info($"Updating customer ID: {customerId}");

            var user = await _userManager.FindByEmailAsync(customer.Email);
            if (user == null)
            {
                _logger.Warn($"Update failed: No customer found with email {customer.Email}");
                return NotFound();
            }

            customer.Adapt(user);
            _userManager.Update(user);

            var response = user.Adapt<CustomerReponse>();

            _logger.Info($"Customer {customerId} updated successfully.");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error updating customer {customerId}: {ex.Message}");
            return BadRequest("Something went wrong while updating customer.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("{customerId}")]
    public async Task<IHttpActionResult> DeleteCustomer(string customerId)
    {
        try
        {
            _logger.Info($"Deleting customer ID: {customerId}");

            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.Warn($"Delete failed: No customer found with ID {customerId}");
                return NotFound();
            }

            var user = await _userManager.FindByEmailAsync(customer.Email);
            if (user == null)
            {
                _logger.Warn($"Delete failed: No user found with email {customer.Email}");
                return NotFound();
            }

            _userManager.Delete(user);
            _logger.Info($"Customer {customerId} deleted successfully.");

            return Ok("Customer deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.Error($"Error deleting customer {customerId}: {ex.Message}");
            return BadRequest("Something went wrong while deleting the customer.");
        }
    }
}
