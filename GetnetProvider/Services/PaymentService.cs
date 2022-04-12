using GetnetProvider.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GetnetProvider.Services
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly GetnetSettings _settings;
        private readonly AuthenticationService _authenticationService;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IOptions<GetnetSettings> settingsOptions,
            AuthenticationService authenticationService,
            ILogger<PaymentService> logger)
        {
            //var httpHandler = new HttpClientHandler()
            //{
            //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            //};

            _httpClient = new HttpClient();
            _settings = settingsOptions.Value;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public async Task<ServiceCommandResponse<CreateCreditPaymentResponse>> PaymentByCreditCard(CreateCreditPaymentRequest paymentRequest)
        {
            try
            {
                var accessToken = await _authenticationService.GetAccessToken();
                var json = JsonSerializer.Serialize(paymentRequest, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);

                var response = await _httpClient.PostAsync($"{_settings.ApiUrl}v1/payments/credit", stringContent);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var paymentResponse = JsonSerializer.Deserialize<CreateCreditPaymentResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<CreateCreditPaymentResponse>(paymentResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Validation.Keys.Select(k => k);
                            return new ServiceCommandResponse<CreateCreditPaymentResponse>((int)response.StatusCode, message, items);
                        };
                    default:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<CreateCreditPaymentResponse>((int)response.StatusCode, message, items);
                        }
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
