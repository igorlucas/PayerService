#nullable disable
using API.Models;
using GetnetProvider.Models;
using GetnetProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly GetnetSettings _getnetSettings;

        public CustomersController(CustomerService customerService, IOptions<GetnetSettings> settingsOptions)
        {
            _customerService = customerService;
            _getnetSettings = settingsOptions.Value;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<GenericApiResponseEntityList<Customer>>> GetCustomers(string query)
        {
            var listCustomerResponse = await _customerService.ListAsync(query);
            var apiResponse = new GenericApiResponseEntityList<Customer>(listCustomerResponse.StatusCode, listCustomerResponse.Message, listCustomerResponse.Result?.Customers);

            if (apiResponse.StatusCode == (int)HttpStatusCode.BadRequest)
                return BadRequest(apiResponse.Message);

            if (apiResponse.StatusCode == (int)HttpStatusCode.Unauthorized)
                return Unauthorized(apiResponse.Message);


            return Ok(apiResponse.Message);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericApiResponseEntity<Customer>>> GetCustomer(string id)
        {
            var serviceResponse = await _customerService.ReadAsync(id);
            var apiResponse = new GenericApiResponseEntity<ReadCustomerResponse>(serviceResponse.StatusCode, serviceResponse.Message, serviceResponse.Result);
            if (apiResponse.StatusCode == (int)HttpStatusCode.NotFound) return NotFound(apiResponse.Message);
            else if (apiResponse.StatusCode == (int)HttpStatusCode.Unauthorized) return Unauthorized(apiResponse.Message);
            else return Ok(apiResponse.Message);
        }


        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GenericApiResponseEntityList<Customer>>> PostCustomer(Customer customer)
        {
            var createCustomerRequest = new CreateCustomerRequest(_getnetSettings.SellerId, customer);
            var createCustomerResponse = await _customerService.CreateAsync(createCustomerRequest);
            var apiResponse = new GenericApiResponseEntity<CreateCustomerResponse>(createCustomerResponse.StatusCode, createCustomerResponse.Message, createCustomerResponse.Result);

            switch (createCustomerResponse.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
                case (int)HttpStatusCode.Unauthorized:
                    return Unauthorized(apiResponse.Message);
                case (int)HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(apiResponse.Message);
                default:
                    return new StatusCodeResult(createCustomerResponse.StatusCode);
            }
        }
    }
}