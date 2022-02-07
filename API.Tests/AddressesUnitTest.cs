using API.Controllers;
using API.Data;
using API.Entities;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace @Addresses
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AddressServiceUnitTests
    {
        private readonly IAddressService _addressService;
        public AddressServiceUnitTests()
        {
            var optionsDbContext = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase($"ServiceInMemoryDB-{Guid.NewGuid}").Options;
            var dbContext = new DataContext(optionsDbContext, migrate: false);
            var addressRepository = new Repository<Address>(dbContext);

            _addressService = new AddressService(addressRepository);
        }

        [Fact, Priority(0)]
        public async Task CreateAddress()
        {
            //// Arrange
            var address = new Address("Rua 1", "88", "", "Messejana", "Fortaleza", "CE", "Brasil", "60872684");

            //// Act        
            var updatedRows = await _addressService.CreateAsync(address);

            //// Assert
            var result = updatedRows > 0;
            Assert.True(result);
        }

        [Fact, Priority(1)]
        public async Task ReadAllAddresses()
        {
            //// Arrange
            //// Act        
            var response = await _addressService.ReadAllAsync();

            //// Assert 
            var addresses = response.Resources;
            var statusOk = response.StatusCode == ((int)HttpStatusCode.OK);
            var result = !Object.ReferenceEquals(addresses, null);
            Assert.True(statusOk);
            Assert.True(result);
        }

        [Fact, Priority(2)]
        public async Task ReadAddressById()
        {
            //// Arrange
            var addressesResponse = await _addressService.ReadAllAsync();
            var addressId = addressesResponse.Resources.First().Id;

            //// Act     
            var addressResponse = await _addressService.ReadByIdAsync(addressId);
            var address = addressResponse.Resource;

            //// Assert
            var statusOk = addressResponse.StatusCode == ((int)HttpStatusCode.OK);
            var result = !ReferenceEquals(address, null);
            Assert.True(statusOk);
            Assert.True(result);
        }

        [Fact, Priority(2)]
        public async Task UpdateAddress()
        {
            //// Arrange
            var addressesResponse = await _addressService.ReadAllAsync();
            var address = addressesResponse.Resources.First();

            //// Act        
            address.City = "Fortim";
            address.District = "Pontal do Maceio";
            var updatedRows = await _addressService.UpdateAsync(address);

            //// Assert
            var result = updatedRows > 0;
            Assert.True(result);
        }

        [Fact, Priority(3)]
        public async Task DeleteAddress()
        {
            //// Arrange
            var addressesResponse = await _addressService.ReadAllAsync();
            var address = addressesResponse.Resources.First();

            //// Act        
            var updatedRows = await _addressService.DeleteAsync(address);

            //// Assert
            var response = await _addressService.ReadByIdAsync(address.Id);
            var result = Object.ReferenceEquals(response.Resource, null) && updatedRows > 0;
            Assert.True(result);
        }
    }

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AddressControllerUnitTests
    {
        private readonly IAddressService _addressService;

        public AddressControllerUnitTests()
        {
            var optionsDbContext = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase($"ControllerInMemoryDB-{Guid.NewGuid}").Options;
            var dbContext = new DataContext(optionsDbContext, false);
            var addressRepository = new Repository<Address>(dbContext);

            _addressService = new AddressService(addressRepository);
        }

        [Fact, Priority(0)]
        public async Task PostAddressWithSuccess()
        {
            // Arrange
            var address = new Address("Rua 1", "88", "", "Messejana", "Fortaleza", "CE", "Brasil", "60872684");
            var controller = new AddressesController(_addressService);

            // Act  
            var response = await controller.PostAddress(address);

            // Assert
            var objectResult = Assert.IsType<CreatedAtActionResult>(response.Result);
            var createdAddress = Assert.IsType<Address>(objectResult.Value);
            Assert.NotNull(createdAddress);
            Assert.IsType<CreatedAtActionResult>(response.Result);
        }

        [Fact, Priority(1)]
        public async Task GetAddressesWithSuccess()
        {
            // Arrange
            var controller = new AddressesController(_addressService);

            // Act  
            var response = await controller.GetAddresses();

            // Assert   
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var addresses = Assert.IsType<GenericApiResponseEntityList<Address>>(objectResult.Value);
            Assert.NotNull(addresses);
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact, Priority(2)]
        public async Task GetAddressWithSuccess()
        {
            // Arrange
            var controller = new AddressesController(_addressService);

            // Act       
            var responseAddresses = await controller.GetAddresses();
            var addressResult = Assert.IsType<OkObjectResult>(responseAddresses.Result);
            var addressId = Assert.IsType<GenericApiResponseEntityList<Address>>(addressResult.Value).Resources.First().Id;
            var response = await controller.GetAddress(addressId);

            // Assert   
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var address = Assert.IsType<GenericApiResponseEntity<Address>>(objectResult.Value);
            Assert.NotNull(address);
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact, Priority(3)]
        public async Task PutAddressWithSuccess()
        {
            // Arrange
            var controller = new AddressesController(_addressService);

            // Act          
            var responseAddresses = await controller.GetAddresses();
            var addressResult = Assert.IsType<OkObjectResult>(responseAddresses.Result);
            var address = (Assert.IsType<GenericApiResponseEntityList<Address>>(addressResult.Value)).Resources.First();
            address.City = "Fortim";
            address.District = "Portal do Maaceio";
            var response = await controller.PutAddress(address.Id, address);

            // Assert   
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact, Priority(4)]
        public async Task DeleteCustomerWithSuccess()
        {
            // Arrange
            var controller = new AddressesController(_addressService);

            // Act      
            var responseAddresses = await controller.GetAddresses();
            var addressResult = Assert.IsType<OkObjectResult>(responseAddresses.Result);
            var address = (Assert.IsType<GenericApiResponseEntityList<Address>>(addressResult.Value)).Resources.First();
            var response = await controller.DeleteAddress(address.Id);

            // Assert   
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.IsType<NoContentResult>(noContentResult);
        }
    }

}