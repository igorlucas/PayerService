using GetnetProvider.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GetnetProvider.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly GetnetSettings _settings;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IOptions<GetnetSettings> settingsOptions, ILogger<AuthenticationService> logger)
        {
            var httpHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(httpHandler);
            _settings = settingsOptions.Value;
            _logger = logger;
        }

        public async Task<GetIPDeviceResponse> GetIPDevice()
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.ipify.org/?format=json");
                var getIPDeviceResponse = JsonSerializer.Deserialize<GetIPDeviceResponse>(await response.Content.ReadAsStringAsync());
                return getIPDeviceResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<CreateAccessTokenResponse> GetAccessToken()
        {
            try
            {
                var authString = GenerateAuthString();

                var stringContent = new StringContent("scope=oob&grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync($"{_settings.ApiUrl}/auth/oauth/v2/token", stringContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var createAccessTokenResponse = JsonSerializer.Deserialize<CreateAccessTokenResponse>(await response.Content.ReadAsStringAsync());
                    return createAccessTokenResponse;
                }
                else
                {
                    var createAccessTokenErrorResponse = JsonSerializer.Deserialize<CreateAccessTokenErrorResponse>(await response.Content.ReadAsStringAsync());
                    return new CreateAccessTokenResponse()
                    {
                        ErrorResponse = createAccessTokenErrorResponse
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceCommandResponse<CreateCardTokenResponse>> TokenizationCard(CreateCardTokenRequest createCardTokenRequest)
        {
            try
            {
                var accessToken = await GetAccessToken();
                var json = JsonSerializer.Serialize(createCardTokenRequest, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);
                _httpClient.DefaultRequestHeaders.Add("seller_id", _settings.SellerId);

                var response = await _httpClient.PostAsync($"{_settings.ApiUrl}/v1/tokens/card", stringContent);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var createCardTokenResponse = JsonSerializer.Deserialize<CreateCardTokenResponse>((await response.Content.ReadAsStringAsync()));
                            _logger.LogInformation($"Novo cartão tokenizado: {createCardTokenRequest}; tokencard: {createCardTokenResponse?.NumberToken}");
                            return new ServiceCommandResponse<CreateCardTokenResponse>(createCardTokenResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Validation.Keys.Select(k => k);
                            _logger.LogWarning("");
                            return new ServiceCommandResponse<CreateCardTokenResponse>((int)response.StatusCode, message, items);
                        };
                    default:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            _logger.LogWarning("");
                            return new ServiceCommandResponse<CreateCardTokenResponse>((int)response.StatusCode, message, items);
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceCommandResponse<CardVerificationResponse>> CheckCard(CardVerificationRequest cardVerificationRequest)
        {
            try
            {
                var accessToken = await GetAccessToken();
                var json = JsonSerializer.Serialize(cardVerificationRequest, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);
                _httpClient.DefaultRequestHeaders.Add("seller_id", _settings.SellerId);

                var response = await _httpClient.PostAsync($"{_settings.ApiUrl}/v1/cards/verification", stringContent);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var cardVerificationResponse = JsonSerializer.Deserialize<CardVerificationResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<CardVerificationResponse>(cardVerificationResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Validation.Keys.Select(k => k);
                            return new ServiceCommandResponse<CardVerificationResponse>((int)response.StatusCode, message, items);
                        };
                    default:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<CardVerificationResponse>((int)response.StatusCode, message, items);
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }
        private string GenerateAuthString() => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}"));
    }
}