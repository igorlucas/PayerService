#nullable disable
using API.Entities;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService) => _customerService = customerService;

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<GenericApiResponseEntityList<Customer>>> GetCustomers() => await _customerService.ReadAllAsync();

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericApiResponseEntity<Customer>>> GetCustomer(Guid id)
        {
            var response = await _customerService.ReadByIdAsync(id);
            if (response == null) return NotFound();
            else return Ok(response);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();

            try
            {
                await _customerService.UpdateAsync(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_customerService.CustomerExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await _customerService.CreateAsync(customer);
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = (await _customerService.ReadByIdAsync(id)).Resource;
            if (customer == null) return NotFound();

            await _customerService.DeleteAsync(customer);

            return NoContent();
        }
    }
}