using Braintree.NETCore.API.Handlers;
using Braintree.NETCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Braintree.NETCore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BraintreeController : Controller
    {
        private readonly IGatewayHandler _handler;
        private readonly ILogger _logger;

        public BraintreeController(IGatewayHandler handler, ILogger<BraintreeController> log)
        {
            _handler = handler;
            _logger = log;
        }

        [HttpGet]
        public IEnumerable<string> GetClientToken()
        {
            _logger.LogDebug("Getting client_token from braintree");
            var generatedToken = _handler.GetClientToken();
            return new string[] { "client_token", generatedToken };
        }

        [HttpPost]
        public JsonResult ProcessPayment([FromBody]PurchaseData form_data)
        {
            _logger.LogDebug("Procesing purchase");
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var paymentResult = _handler.ProcessPayment(form_data);
            if (paymentResult.IsSuccess())
                return Json(paymentResult.Target);
            else
                return Json(paymentResult.Message);
        }
    }
}
