using Braintree.NETCore.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Braintree.NETCore.API.Handlers
{
    public class GatewayHandler : IGatewayHandler
    {
        private IBraintreeGateway _Gateway;
        public IBraintreeGateway Gateway { get => _Gateway; }

        private IConfiguration _configuration;
        private ILogger _log;

        public GatewayHandler(IConfiguration configuration, ILogger<GatewayHandler> logger)
        {
            _configuration = configuration;
            _log = logger;
            _Gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = _configuration["Braintree:MerchantID"],
                PublicKey = _configuration["Braintree:PublicKey"],
                PrivateKey = _configuration["Braintree:PrivateKey"]
            };
        }

        public string GetClientToken()
        {
            return Gateway.ClientToken.generate();
        }

        public Result<Transaction> ProcessPayment(PurchaseData data)
        {
            try
            {
                var request = new TransactionRequest
                {
                    Amount = data.amount,
                    PaymentMethodNonce = data.payment_method_nonce,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                return Gateway.Transaction.Sale(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("Braintree Error: " + e.Message);
                return null;
            }
        }
    }
}
