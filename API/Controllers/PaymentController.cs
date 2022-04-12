using API.Models;
using API.Models.CommandRequests;
using GetnetProvider.Models;
using GetnetProvider.Models.Enums;
using GetnetProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly AuthenticationService _authenticationService;
        private readonly IOptions<GetnetSettings> _settingsOptions;

        public PaymentController(AuthenticationService authenticationService, PaymentService paymentService, IOptions<GetnetSettings> settingsOptions)
        {
            _paymentService = paymentService;
            _authenticationService = authenticationService;
            _settingsOptions = settingsOptions;
        }

        [HttpPost("credit")]
        public async Task<ActionResult<GenericApiResponseEntity<CreateCreditPaymentResponse>>> PaymentByCredit(PaymentCommandRequest commandRequest)
        {
            try
            {
                var tokenResponse = (await _authenticationService.TokenizationCard(new CreateCardTokenRequest(commandRequest.CardNumber, commandRequest.Customer.Id)))?.Result;
                var IP = (await _authenticationService.GetIPDevice()).IP;
                var paymentResponse = await _paymentService.PaymentByCreditCard(new CreateCreditPaymentRequest
                {
                    Amount = commandRequest.Amount,
                    Credit = new PaymentRequestCreditData(commandRequest.SaveCardData,
                    $"{TransactionTypeEnum.FULL}",
                    commandRequest.NumberInstallments,
                    commandRequest.SoftDescriptor,
                    new PaymentRequestCard(tokenResponse.NumberToken, commandRequest.CardholderName, commandRequest.SecurityCode, null, commandRequest.ExpirationMonth, commandRequest.ExpirationYear)),
                    Currency = "BRL",
                    Customer = commandRequest.Customer,
                    Device = new PaymentRequestDevice(IP, _settingsOptions.Value.SellerId),
                    Order = commandRequest.Order,
                    SellerId = _settingsOptions.Value.SellerId,
                });
                var apiResponse = new GenericApiResponseEntity<CreateCreditPaymentResponse>(paymentResponse.StatusCode, paymentResponse.Message, paymentResponse.Result);
                return Ok(apiResponse);
                //return CreatedAtAction(nameof(GetPayment), new { id = apiResponse.Resource.PaymentId }, apiResponse.Resource);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}