using GetnetProvider.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GetnetProvider.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly GetnetSettings _settings;
        private readonly AuthenticationService _authenticationService;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            IOptions<GetnetSettings> settingsOptions,
            AuthenticationService authenticationService,
            ILogger<CustomerService> logger)
        {
            var httpHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(httpHandler);
            _settings = settingsOptions.Value;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public async Task<ServiceCommandResponse<CreateCustomerResponse>> CreateAsync(CreateCustomerRequest createCustomerRequest)
        {
            try
            {
                var accessToken = await _authenticationService.GetAccessToken();
                var json = JsonSerializer.Serialize(createCustomerRequest, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);

                var response = await _httpClient.PostAsync($"{_settings.ApiUrl}/v1/customers", stringContent);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var createCustomerResponse = JsonSerializer.Deserialize<CreateCustomerResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<CreateCustomerResponse>(createCustomerResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Validation.Keys.Select(k => k);
                            return new ServiceCommandResponse<CreateCustomerResponse>((int)response.StatusCode, message, items);
                        };
                    default:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<CreateCustomerResponse>((int)response.StatusCode, message, items);
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<ServiceCommandResponse<ListCustomerResponse>> ListAsync(string queryString)
        {
            try
            {
                var accessToken = await _authenticationService.GetAccessToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);
                _httpClient.DefaultRequestHeaders.Add("seller_id", _settings.SellerId);

                var response = await _httpClient.GetAsync($"{_settings.ApiUrl}/v1/customers{queryString}");

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var listCustomerResponse = JsonSerializer.Deserialize<ListCustomerResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<ListCustomerResponse>(listCustomerResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}. Source: {errorScheme.Validation.Source}";
                            return new ServiceCommandResponse<ListCustomerResponse>((int)response.StatusCode, message, errorScheme.Validation.Keys);
                        };
                    default:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<ListCustomerResponse>((int)response.StatusCode, message, items);
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<ServiceCommandResponse<ReadCustomerResponse>> ReadAsync(string customerId)
        {
            try
            {
                var accessToken = await _authenticationService.GetAccessToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);
                _httpClient.DefaultRequestHeaders.Add("seller_id", _settings.SellerId);

                var response = await _httpClient.GetAsync($"{_settings.ApiUrl}/v1/customers/{customerId}");

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var readCustomerResponse = JsonSerializer.Deserialize<ReadCustomerResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<ReadCustomerResponse>(readCustomerResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}. Source: {errorScheme.Validation.Source}";
                            return new ServiceCommandResponse<ReadCustomerResponse>((int)response.StatusCode, message, errorScheme.Validation.Keys);
                        };
                    case HttpStatusCode.NotFound:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<string>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e);
                            return new ServiceCommandResponse<ReadCustomerResponse>((int)response.StatusCode, message, items);
                        };
                    default:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<ReadCustomerResponse>((int)response.StatusCode, message, items);
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}