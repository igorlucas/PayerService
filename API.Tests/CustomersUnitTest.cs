using API.Controllers;
using API.Data;
using API.Entities;
using API.Models;
using API.Models.Enums;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace @Customers
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class CustomerServiceUnitTests
    {
        private readonly ICustomerService _customerService;
        public CustomerServiceUnitTests()
        {
            var optionsDbContext = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase($"ServiceInMemoryDB-{Guid.NewGuid}").Options;
            var dbContext = new DataContext(optionsDbContext, migrate: false);
            var customerRepository = new Repository<Customer>(dbContext);

            _customerService = new CustomerService(customerRepository);
        }

        [Fact, Priority(0)]
        public async Task CreateCustomer()
        {
            //// Arrange
            var address = new Address("Rua 1", "88", "", "Messejana", "Fortaleza", "CE", "Brasil", "60872684");
            var customer = new Customer("Igor Lucas Silva", "CPF", "46199345002", new DateTime(1992, 12, 14), "", "85985717711", "igor@email.com", address);

            //// Act        
            var updatedRows = await _customerService.CreateAsync(customer);

            //// Assert
            var result = updatedRows > 0;
            Assert.True(result);
        }

        [Fact, Priority(1)]
        public async Task ReadAllCustomers()
        {
            //// Arrange
            //// Act        
            var response = await _customerService.ReadAllAsync();

            //// Assert
            var customers = response.Resources;
            var statusOk = response.StatusCode == ((int)HttpStatusCode.OK);
            var result = !Object.ReferenceEquals(customers, null);
            Assert.True(statusOk);
            Assert.True(result);
        }

        [Fact, Priority(2)]
        public async Task ReadCustomerById()
        {
            //// Arrange
            var customersResponse = await _customerService.ReadAllAsync();
            var customerId = customersResponse.Resources.First().Id;

            //// Act    
            var customerResponse = await _customerService.ReadByIdAsync(customerId);
            var customer = customerResponse.Resource;

            //// Assert
            var statusOk = customerResponse.StatusCode == ((int)HttpStatusCode.OK);
            var result = !ReferenceEquals(customer, null);
            Assert.True(statusOk);
            Assert.True(result);
        }

        [Fact, Priority(2)]
        public async Task UpdateCustomer()
        {
            //// Arrange
            var customersResponse = await _customerService.ReadAllAsync();
            var customer = customersResponse.Resources.First();

            //// Act        
            customer.DocumentType = DocumentType.CNPJ.ToString();
            customer.DocumentValue = "80525028000181";
            var updatedRows = await _customerService.UpdateAsync(customer);

            //// Assert
            var result = updatedRows > 0;
            Assert.True(result);
        }

        [Fact, Priority(3)]
        public async Task DeleteCustomer()
        {
            //// Arrange
            var customersResponse = await _customerService.ReadAllAsync();
            var customer = customersResponse.Resources.First();

            //// Act        
            var updatedRows = await _customerService.DeleteAsync(customer);

            //// Assert
            var response = await _customerService.ReadByIdAsync(customer.Id);
            var result = Object.ReferenceEquals(response.Resource, null) && updatedRows > 0;
            Assert.True(result);
        }
    }

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class CustomerControllerUnitTests
    {
        private readonly CustomerService _customerService;

        public CustomerControllerUnitTests()
        {
            var optionsDbContext = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase($"ControllerInMemoryDB-{Guid.NewGuid}").Options;
            var dbContext = new DataContext(optionsDbContext, false);
            var customerRepository = new Repository<Customer>(dbContext);

            _customerService = new CustomerService(customerRepository);
        }

        [Fact, Priority(0)]
        public async Task PostCustomerWithSuccess()
        {
            // Arrange
            var address = new Address("Rua 1", "88", "", "Messejana", "Fortaleza", "CE", "Brasil", "60872684");
            var customer = new Customer("Igor Lucas Silva", "CPF", "46199345002", new DateTime(1992, 12, 14), "", "85985717711", "igor@email.com", address);
            var controller = new CustomersController(_customerService);

            // Act  
            var response = await controller.PostCustomer(customer);

            // Assert
            var objectResult = Assert.IsType<CreatedAtActionResult>(response.Result);
            var createdCustomer = Assert.IsType<Customer>(objectResult.Value);
            Assert.NotNull(createdCustomer);
            Assert.IsType<CreatedAtActionResult>(response.Result);
        }

        [Fact, Priority(1)]
        public async Task GetCustomersWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act  
            var response = await controller.GetCustomers();

            // Assert   
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var customers = Assert.IsType<GenericApiResponseEntityList<Customer>>(objectResult.Value);
            Assert.NotNull(customers);
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact, Priority(2)]
        public async Task GetCustomerWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act      
            var responseCustomers = await controller.GetCustomers();
            var customerResult = Assert.IsType<OkObjectResult>(responseCustomers.Result);
            var customerId = Assert.IsType<GenericApiResponseEntityList<Customer>>(customerResult.Value).Resources.First().Id;
            var response = await controller.GetCustomer(customerId);

            // Assert   
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var customer = Assert.IsType<GenericApiResponseEntity<Customer>>(objectResult.Value);
            Assert.NotNull(customer);
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact, Priority(3)]
        public async Task PutCustomerWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act      
            var responseCustomers = await controller.GetCustomers();
            var customerResult = Assert.IsType<OkObjectResult>(responseCustomers.Result);
            var customer = (Assert.IsType<GenericApiResponseEntityList<Customer>>(customerResult.Value)).Resources.First();
            customer.FullName = "Igor Epfanio Silva";
            var response = await controller.PutCustomer(customer.Id, customer);

            // Assert   
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact, Priority(4)]
        public async Task DeleteCustomerWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act      
            var responseCustomers = await controller.GetCustomers();
            var customerResult = Assert.IsType<OkObjectResult>(responseCustomers.Result);
            var customer = (Assert.IsType<GenericApiResponseEntityList<Customer>>(customerResult.Value)).Resources.First();
            var response = await controller.DeleteCustomer(customer.Id);

            // Assert   
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.IsType<NoContentResult>(noContentResult);
        }
    }
}