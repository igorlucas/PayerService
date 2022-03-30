using GetnetProvider.Models;
using GetnetProvider.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace @Getnet
{
    public class AuthenticationServiceUnitTests
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IOptions<GetnetSettings> _getnetSettingsOptions;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceUnitTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            _getnetSettingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["SellerId"],
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            _authenticationService = new AuthenticationService(_getnetSettingsOptions);
        }

        [Fact]
        public async Task GetAccessToken_WhenCredentialsAreValid_ShouldReturnAToken()
        {
            //// Arrange
            //// Act        
            var response = await _authenticationService.GetAccessToken();

            //// Assert
            Assert.Null(response.ErrorResponse);
            Assert.NotNull(response.Token);
        }

        [Theory]
        [InlineData("cli-123")]
        public async Task GetAccessToken_WhenClientIdIsInValid_ShouldReturnAnErrorResponse(string clientId)
        {
            //// Arrange
            var getnetSettingsOptions = Options.Create(new GetnetSettings()
            {
                ClientId = clientId,
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            var authenticationService = new AuthenticationService(getnetSettingsOptions);

            //// Act        
            var response = await authenticationService.GetAccessToken();

            //// Assert
            Assert.NotNull(response.ErrorResponse);
        }

        [Theory]
        [InlineData("cli-sec-123")]
        public async Task GetAccessToken_WhenClientSecretIsInValid_ShouldReturnAnErrorResponse(string clientSecrete)
        {
            //// Arrange
            var getnetSettingsOptions = Options.Create(new GetnetSettings()
            {
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = clientSecrete,
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            var authenticationService = new AuthenticationService(getnetSettingsOptions);

            //// Act        
            var response = await authenticationService.GetAccessToken();

            //// Assert
            Assert.NotNull(response.ErrorResponse);
        }
    }

    public class CustomerServiceUnitTests
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IOptions<GetnetSettings> _settingsOptions;
        private readonly AuthenticationService _authenticationService;
        private readonly ILogger<CustomerService> _logger;
        private readonly CustomerService _customerService;

        public CustomerServiceUnitTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            _settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["SellerId"],
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"]
            });

            _authenticationService = new AuthenticationService(_settingsOptions);

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _logger = factory.CreateLogger<CustomerService>();

            _customerService = new CustomerService(_settingsOptions, _authenticationService, _logger);
        }

        #region CreateCustomer
        [Fact]
        public async Task CreateCustomer_WhenRequestBodyIsValid_ShouldReturnAResultBodyWithoutError()
        {
            //// Arrange
            var sellerId = _settingsOptions.Value.SellerId;
            var observation = "O cliente tem interesse no plano x.";
            var address = new Address("Av. Brasil", "1000", "Sala 1", "São Geraldo", "Porto Alegre", "RS", "Brasil", "90230060");
            var customer = new Customer("João", "da Silva", "CPF", "12345678912", "5551999887766", "5551999887766", "customer@email.com.br", "1976-02-21", address);
            customer.Id = "customer_21081826";
            var createCustomerRequest = new CreateCustomerRequest(sellerId, observation, customer);

            //// Act    
            var createCustomerResponse = await _customerService.CreateAsync(createCustomerRequest);

            //// Assert
            var response = !Object.ReferenceEquals(createCustomerResponse, null);
            var resultBody = !Object.ReferenceEquals(createCustomerResponse.Result, null);
            Assert.True(response);
            Assert.True(resultBody);
        }

        [Fact]
        public async Task CreateCustomer_WhenRequestBodyIsNotValid_ShouldReturnAResponseError()
        {
            //// Arrange
            //// Arrange
            var sellerId = _settingsOptions.Value.SellerId;
            var observation = "O cliente tem interesse no plano x.";
            var address = new Address("Av. Brasil", "1000", "Sala 1", "São Geraldo", "Porto Alegre", "RS", "Brasil", "90230060");
            var customer = new Customer("João", "da Silva", "CPF", string.Empty, "5551999887766", "5551999887766", "customer@email.com.br", "1976-02-21", address);
            customer.Id = "customer_21081826";
            var createCustomerRequest = new CreateCustomerRequest(sellerId, observation, customer);

            //// Act    
            var createCustomerResponse = await _customerService.CreateAsync(createCustomerRequest);

            //// Assert
            Assert.NotNull(createCustomerResponse);
            Assert.Null(createCustomerResponse.Result);
            Assert.NotNull(createCustomerResponse.Error);
        }

        [Fact]
        public async Task CreateCustomer_WhenSellerIDIsNotValid_ShouldReturnAResponseError()
        {
            //// Arrange
            var sellerId = Guid.NewGuid().ToString();
            var observation = "O cliente tem interesse no plano x.";
            var address = new Address("Av. Brasil", "1000", "Sala 1", "São Geraldo", "Porto Alegre", "RS", "Brasil", "90230060");
            var customer = new Customer("João", "da Silva", "CPF", string.Empty, "5551999887766", "5551999887766", "customer@email.com.br", "1976-02-21", address);
            customer.Id = "customer_21081826";
            var createCustomerRequest = new CreateCustomerRequest(sellerId, observation, customer);

            //// Act    
            var createCustomerResponse = await _customerService.CreateAsync(createCustomerRequest);

            //// Assert
            //// Assert
            Assert.NotNull(createCustomerResponse);
            Assert.Null(createCustomerResponse.Result);
            Assert.NotNull(createCustomerResponse.Error);
        }
        #endregion CreateCustomer

        #region ListCustomers
        [Fact]
        public async Task ListCustomers_WhenQueryStringIsValid_ShouldReturnAListOfCustomers()
        {
            //// Arrange
            int page = 1, limit = 10;
            var queryString = $"?page={page}&limit={limit}";

            //// Act            
            var listCustomerResponse = await _customerService.ListAsync(queryString);

            //// Assert
            Assert.NotNull(listCustomerResponse.Result);
            Assert.NotNull(listCustomerResponse);
            Assert.Null(listCustomerResponse.Error);
        }

        [Fact]
        public async Task ListCustomers_WhenQueryStringIsNotValid_ShouldReturnAResponseError()
        {
            //// Arrange
            int page = 0, limit = 10;
            var queryString = $"?page={page}&limit={limit}";

            //// Act            
            var listCustomersResponse = await _customerService.ListAsync(queryString);

            //// Assert
            Assert.NotNull(listCustomersResponse);
            Assert.NotNull(listCustomersResponse.Error);
            Assert.True(listCustomersResponse.Error.Items.Any());
        }

        [Fact]
        public async Task ListCustomers_WhenCredentialIsNotValid_ShouldReturnAResponseError()
        {
            //// Arrange
            var settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = Guid.NewGuid().ToString(),
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            var authenticationService = new AuthenticationService(settingsOptions);

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<CustomerService>();

            var customerService = new CustomerService(settingsOptions, authenticationService, logger);

            int page = 1, limit = 10;

            var queryString = $"?page={page}&limit={limit}";

            //// Act                
            var listCustomersResponse = await customerService.ListAsync(queryString);

            //// Assert
            Assert.NotNull(listCustomersResponse);
            Assert.NotNull(listCustomersResponse.Error);
            Assert.True(listCustomersResponse.Error.Items.Any());
            Assert.Null(listCustomersResponse.Result);
        }
        #endregion ListCustomers

        #region ReadCustomerById
        [Fact]
        public async Task ReadCustomer_WhenThereIsOneForID_ShouldReturnACustomer()
        {
            //// Arrange
            var customerId = "customer_21081826";

            //// Act            
            var readCustomerResponse = await _customerService.ReadAsync(customerId);

            //// Assert
            Assert.NotNull(readCustomerResponse);
            Assert.NotNull(readCustomerResponse.Result);
            Assert.Null(readCustomerResponse.Error);
        }

        [Fact]
        public async Task ReadCustomer_WhenCredentialIsNotValid_ShouldReturnAResponseError()
        {
            //// Arrange
            var settingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = Guid.NewGuid().ToString(),
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            var authenticationService = new AuthenticationService(settingsOptions);

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<CustomerService>();

            var customerService = new CustomerService(settingsOptions, authenticationService, logger);

            var customer_id = $"123";

            //// Act            
            var readCustomerResponse = await customerService.ReadAsync(customer_id);

            //// Assert
            Assert.NotNull(readCustomerResponse);
            Assert.NotNull(readCustomerResponse.Error);
            Assert.Null(readCustomerResponse.Result);
        }

        [Fact]
        public async Task ReadCustomer_WhenIsNotFound_ShouldReturnAResponseError()
        {
            //// Arrange
            var customerId = "123";

            //// Act            
            var readCustomerResponse = await _customerService.ReadAsync(customerId);

            //// Assert
            Assert.NotNull(readCustomerResponse);
            Assert.NotNull(readCustomerResponse.Error);
            Assert.Null(readCustomerResponse.Result);
        }
        #endregion ReadCustomerById
    }
}