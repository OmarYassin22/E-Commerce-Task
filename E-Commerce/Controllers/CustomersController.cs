using E_Commerce.Contract;
using E_Commerce.Contract.ReposContract;
using E_Commerce.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
[RoutePrefix("api/customers")]
public class CustomersController : ApiController
{
    private readonly IRepo<Customer> _repo;

    public CustomersController(IRepo<Customer> repo)
    {
        _repo = repo;
    }

    // GET: api/customers
    [HttpGet]
    [Route("")]
    public async Task<IHttpActionResult> Get()
    {
        var customers = await _repo.GetAllAsync();
        return Ok(customers.Adapt<IEnumerable<CustomerRequest>>());
    }

    // GET: api/customers/5
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IHttpActionResult> Get(int id)
    {
        var res = await _repo.GetByIdAsync(id);
        if (res == null)
            return NotFound();

        return Ok(res.Adapt<CustomerRequest>());
    }


    // POST: api/Customers
    //[HttpPost]
    //[Route("api/customers")]
    //public async Task<IHttpActionResult> Post([FromBody] CustomerRequest value)
    //{
    //    if (value == null)
    //        return BadRequest("Add Valid User");
    //    _repo.Add(value.Adapt<Customer>());
    //    var res = await _repo.SaveAsync();
    //    if (res > 0)
    //        return Ok();
    //    return BadRequest();

    //}

    //// PUT: api/Customers/5
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE: api/Customers/5
    //public void Delete(int id)
    //{
    //}
}

