using API.Controllers;
using API.Models;
using API.Models.CommandRequests;
using GetnetProvider.Models;
using GetnetProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Xunit;

namespace @Getnet
{
    public class CustomerControllerUnitTests
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IOptions<GetnetSettings> _settingsOptions;
        private readonly AuthenticationService _authenticationService;
        private readonly CustomerService _customerService;

        public CustomerControllerUnitTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            _settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["SellerId"],
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"]
            });


            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _authenticationService = new AuthenticationService(_settingsOptions, factory.CreateLogger<AuthenticationService>());

            _customerService = new CustomerService(_settingsOptions, _authenticationService, factory.CreateLogger<CustomerService>());
        }

        #region CreateCustomer
        [Fact]
        public async Task CreateCustomer_WhenRequestBodyIsValid_ShouldReturnACreatedAtActionResult()
        {
            //// Arrange
            var address = new Address("Av. Brasil", "1000", "Sala 1", "São Geraldo", "Porto Alegre", "RS", "Brasil", "90230060");
            var customer = new Customer("João", "da Silva", "CPF", "12345678912", "5551999887766", "5551999887766", "customer@email.com.br", "1976-02-21", address);
            customer.Id = "customer_21081826";

            var controller = new API.Controllers.CustomersController(_customerService, _settingsOptions);

            // Act  
            var response = await controller.PostCustomer(customer);

            // Assert
            var objectResult = Assert.IsType<CreatedAtActionResult>(response.Result);
            var createdCustomer = Assert.IsType<Customer>(objectResult.Value);
            Assert.NotNull(objectResult);
            Assert.NotNull(createdCustomer);
        }

        [Fact]
        public async Task CreateCustomer_WhenRequestBodyIsNotValid_ShouldReturnABadRequestObjectResult()
        {
            //// Arrange
            //// Arrange
            var address = new Address("Av. Brasil", "1000", "Sala 1", "São Geraldo", "Porto Alegre", "RS", "Brasil", "90230060");
            var customer = new Customer("João", "da Silva", "CPF", string.Empty, "5551999887766", "5551999887766", "customer@email.com.br", "1976-02-21", address);
            customer.Id = "customer_21081826";

            var controller = new API.Controllers.CustomersController(_customerService, _settingsOptions);

            // Act  
            var response = await controller.PostCustomer(customer);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            var errorMessage = Assert.IsType<string>(objectResult.Value);
            Assert.NotNull(objectResult);
            Assert.NotNull(errorMessage);
        }

        [Fact]
        public async Task CreateCustomer_WhenSellerIDIsNotValid_ShouldReturnAUnauthorizedResult()
        {
            //// Arrange
            var settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = Guid.NewGuid().ToString(),
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"]
            });

            var address = new Address("Av. Brasil", "1000", "Sala 1", "São Geraldo", "Porto Alegre", "RS", "Brasil", "90230060");
            var customer = new Customer("João", "da Silva", "CPF", "12345678912", "5551999887766", "5551999887766", "customer@email.com.br", "1976-02-21", address);
            customer.Id = "customer_21081826";

            var controller = new API.Controllers.CustomersController(_customerService, settingsOptions);

            // Act  
            var response = await controller.PostCustomer(customer);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(response.Result);
        }
        #endregion CreateCustomer

        #region ListCustomers
        [Fact]
        public async Task ListCustomers_WhenQueryStringIsValid_ShouldReturnAOkObjectResult()
        {
            //// Arrange
            int page = 1, limit = 10;
            var queryString = $"?page={page}&limit={limit}";
            var controller = new API.Controllers.CustomersController(_customerService, _settingsOptions);

            // Act  
            var response = await controller.GetCustomers(queryString);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var message = Assert.IsType<string>(objectResult.Value);
            Assert.NotNull(objectResult);
            Assert.NotNull(message);
        }

        [Fact]
        public async Task ListCustomers_WhenQueryStringIsNotValid_ShouldReturnABadRequestObjectResult()
        {
            //// Arrange
            int page = 0, limit = 10;
            var queryString = $"?page={page}&limit={limit}";
            var controller = new API.Controllers.CustomersController(_customerService, _settingsOptions);

            // Act  
            var response = await controller.GetCustomers(queryString);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            var errorMessage = Assert.IsType<string>(objectResult.Value);
            Assert.NotNull(objectResult);
            Assert.NotNull(errorMessage);
        }

        [Fact]
        public async Task ListCustomers_WhenCredentialIsNotValid_ShouldReturnAUnauthorizedObjectResult()
        {
            //// Arrange
            var settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = Guid.NewGuid().ToString(),
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"]
            });

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var authenticationService = new AuthenticationService(_settingsOptions, factory.CreateLogger<AuthenticationService>());

            var customerService = new CustomerService(settingsOptions, authenticationService, factory.CreateLogger<CustomerService>());

            int page = 1, limit = 10;
            var queryString = $"?page={page}&limit={limit}";
            var controller = new API.Controllers.CustomersController(customerService, settingsOptions);


            //// Act                
            var response = await controller.GetCustomers(queryString);

            //// Assert
            Assert.IsType<UnauthorizedObjectResult>(response.Result);
        }
        #endregion ListCustomers

        #region ReadCustomerById
        [Fact]
        public async Task ReadCustomer_WhenThereIsOneForID_ShouldReturnAOkObjectResult()
        {
            //// Arrange
            var customerId = "customer_21081826";
            var controller = new API.Controllers.CustomersController(_customerService, _settingsOptions);

            // Act  
            var response = await controller.GetCustomer(customerId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var message = Assert.IsType<string>(objectResult.Value);
            Assert.NotNull(objectResult);
            Assert.NotNull(message);
        }

        [Fact]
        public async Task ReadCustomer_WhenCredentialIsNotValid_ShouldReturnAUnauthorizedObjectResult()
        {
            //// Arrange
            var settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = Guid.NewGuid().ToString(),
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"]
            });

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var authenticationService = new AuthenticationService(settingsOptions, factory.CreateLogger<AuthenticationService>());

            var customerService = new CustomerService(settingsOptions, authenticationService, factory.CreateLogger<CustomerService>());

            var controller = new API.Controllers.CustomersController(customerService, settingsOptions);

            var customerId = "customer_21081826";

            // Act  
            var response = await controller.GetCustomer(customerId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(response.Result);
        }

        [Fact]
        public async Task ReadCustomer_WhenIsNotFound_ShouldReturnANotFoundObjectResult()
        {
            //// Arrange
            var customerId = "123";
            var controller = new API.Controllers.CustomersController(_customerService, _settingsOptions);

            // Act  
            var response = await controller.GetCustomer(customerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response.Result);
        }
        #endregion ReadCustomerById

    }

    public class PaymentControllerUnitTests
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IOptions<GetnetSettings> _settingsOptions;
        private readonly AuthenticationService _authenticationService;
        private readonly PaymentService _paymentService;

        public PaymentControllerUnitTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            _settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["SellerId"],
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"]
            });


            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _authenticationService = new AuthenticationService(_settingsOptions, factory.CreateLogger<AuthenticationService>());

            _paymentService = new PaymentService(_settingsOptions, _authenticationService, factory.CreateLogger<PaymentService>());
        }

        [Fact]
        public async Task PaymentByCredit_WhenRequestBodyIsValid_ShouldReturnACreatedAtActionResult()
        {
            //// Arrange
            var request = new PaymentCommandRequest()
            {
                Amount = 100,
                CardholderName = "JOAO DA SILVA",
                CardNumber = "5155901222280001",
                ExpirationMonth = "12",
                ExpirationYear = "28",
                SecurityCode = "123",
                NumberInstallments = 1,
                SaveCardData = false,
                SoftDescriptor = "Service - A",
                Customer = new Customer
                {
                    Id = "customer_21081826",
                    Name = "João da Silva",
                    Firstname = "João",
                    Lastname = "da Silva",
                    Email = "customer@email.com.br",
                    DocumentType = "CPF",
                    DocumentNumber = "05187887897",
                    Phone = "85999887766",
                    BillingAddress = new Address("Av. Brasil", "1000", "Sala 1", "São Geraldo", "Porto Alegre", "RS", "Brasil", "90230060")
                },
                Order = new PaymentRequestOrder()
                {
                    OrderId = "service_123",
                    ProductType = "service",
                    SalesTax = 0
                }
            };

            var controller = new PaymentController(_authenticationService, _paymentService, _settingsOptions);

            // Act  
            var response = await controller.PaymentByCredit(request);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var createdPayment = Assert.IsType<GenericApiResponseEntity<CreateCreditPaymentResponse>>(objectResult.Value);
            Assert.NotNull(objectResult);
            Assert.NotNull(createdPayment);
        }
    }

}