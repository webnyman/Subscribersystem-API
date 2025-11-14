namespace Subscribersystem_API.Models
{
    public class Subscriber
    {
        public string SubscriptionNumber { get; set; } = string.Empty; // sub_prenr
        public string PersonalNumber { get; set; } = string.Empty;     // sub_personnr
        public string FirstName { get; set; } = string.Empty;          // sub_fornamn
        public string LastName { get; set; } = string.Empty;           // sub_efternamn
        public string PhoneNumber { get; set; } = string.Empty;        // sub_telefon
        public string DeliveryAddress { get; set; } = string.Empty;    // sub_adr_utm
        public string PostalCode { get; set; } = string.Empty;         // sub_postnr
        public string City { get; set; } = string.Empty;               // sub_ort
    }
    public class SubscriberContactUpdateDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
