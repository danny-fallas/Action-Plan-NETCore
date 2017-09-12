namespace Braintree.NETCore.API.Models
{
    public class PurchaseData
    {
        public string payment_method_nonce;
        public string client_token;
        public decimal amount;
    }
}
