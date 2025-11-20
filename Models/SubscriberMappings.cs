namespace Subscribersystem_API.Models
{
    public static class SubscriberMappings
    {
        public static SubscriberXmlDto ToXmlDto(this Subscriber s) => new()
        {
            SubscriptionNumber = s.SubscriptionNumber,
            PersonalNumber = s.PersonalNumber,
            FirstName = s.FirstName,
            LastName = s.LastName,
            PhoneNumber = s.PhoneNumber,
            DeliveryAddress = s.DeliveryAddress,
            PostalCode = s.PostalCode,
            City = s.City
        };

        public static Subscriber ToEntity(this SubscriberXmlDto dto) => new()
        {
            SubscriptionNumber = dto.SubscriptionNumber,
            PersonalNumber = dto.PersonalNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            DeliveryAddress = dto.DeliveryAddress,
            PostalCode = dto.PostalCode,
            City = dto.City
        };
    }
}
