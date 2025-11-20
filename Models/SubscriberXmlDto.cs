using System.Xml.Serialization;

namespace Subscribersystem_API.Models
{
    [XmlType("Subscriber")]
    public class SubscriberXmlDto
    {
        public string SubscriptionNumber { get; set; } = string.Empty;
        public string PersonalNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    [XmlRoot("Subscribers")]
    public class SubscriberXmlList
    {
        [XmlElement("Subscriber")]
        public List<SubscriberXmlDto> Items { get; set; } = new();
    }
}
