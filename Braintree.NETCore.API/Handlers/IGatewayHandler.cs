using Braintree.NETCore.API.Models;

namespace Braintree.NETCore.API.Handlers
{
    public interface IGatewayHandler
    {
        IBraintreeGateway Gateway { get; }

        string GetClientToken();
        Result<Transaction> ProcessPayment(PurchaseData data);
    }
}
