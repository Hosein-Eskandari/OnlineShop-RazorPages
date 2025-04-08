using System.Text.Json.Serialization;

namespace _0_Framework.Application.ZarinPal;

public class PaymentRequest
{
    [JsonPropertyName("Mobile")] 
    public string Mobile { get; set; } //mobile -> zarinpal does not understand it!!!

    [JsonPropertyName("Email")] 
    public string Email { get; set; }

    [JsonPropertyName("CallbackURL")] 
    public string CallbackURL { get; set; }

    [JsonPropertyName("Description")] 
    public string Description { get; set; }

    [JsonPropertyName("Amount")] 
    public int Amount { get; set; }

    [JsonPropertyName("MerchantID")] 
    public string MerchantID { get; set; }
}