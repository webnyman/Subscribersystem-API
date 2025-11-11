namespace Subscribersystem_API.Models
{
    public class SubscriberAdInfoDto
    {
        public string SubscriptionNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool AllowedToAdvertise { get; set; }
    }
}
