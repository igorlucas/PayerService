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
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService) => _addressService = addressService;

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<GenericApiResponseEntityList<Address>>> GetAddresses() => await _addressService.ReadAllAsync();

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(Guid id)
        {
            var response = await _addressService.ReadByIdAsync(id);
            if (response == null) return NotFound();
            else return Ok(response);
        }

        // PUT: api/Addresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(Guid id, Address address)
        {
            if (id != address.Id) return BadRequest();

            try
            {
                await _addressService.UpdateAsync(address);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_addressService.AddressExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Addresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            await _addressService.CreateAsync(address);
            return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var response = await _addressService.ReadByIdAsync(id);
            if (response.Resource == null) return NotFound();

            await _addressService.DeleteAsync(response.Resource);

            return NoContent();
        }
    }
}