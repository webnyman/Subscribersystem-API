using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Subscribersystem_API.Data;
using Subscribersystem_API.Models;
using System.Xml.Serialization;

namespace Subscribersystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscribersController : ControllerBase
    {
        private readonly SubscriberContext _context;

        public SubscribersController(SubscriberContext context)
        {
            _context = context;
        }

        // GET: api/subscribers/{subscriptionNumber}
        [HttpGet("{subscriptionNumber}")]
        public async Task<ActionResult<Subscriber>> GetSubscriber(string subscriptionNumber)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.SubscriptionNumber == subscriptionNumber);

            if (subscriber == null)
                return NotFound(new { message = "Prenumerant hittades inte." });

            return Ok(subscriber);
        }
        [HttpGet("{subscriptionNumber}/ad-info")]
        public async Task<ActionResult<SubscriberAdInfoDto>> GetSubscriberAdInfo(string subscriptionNumber)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.SubscriptionNumber == subscriptionNumber);

            if (subscriber == null)
                return NotFound(new { message = "Prenumerant hittades inte." });

            var dto = new SubscriberAdInfoDto
            {
                SubscriptionNumber = subscriber.SubscriptionNumber,
                FullName = $"{subscriber.FirstName} {subscriber.LastName}",
                PhoneNumber = subscriber.PhoneNumber,
                DeliveryAddress = subscriber.DeliveryAddress,
                PostalCode = subscriber.PostalCode,
                City = subscriber.City,
                // Lägga in logik om betalstatus etc.
                AllowedToAdvertise = true
            };

            return Ok(dto);
        }
        [HttpPut("{subscriptionNumber}/contact")]
        public async Task<IActionResult> UpdateContact(
        string subscriptionNumber,
        [FromBody] SubscriberContactUpdateDto dto)
            {
                var subscriber = await _context.Subscribers
                    .FirstOrDefaultAsync(s => s.SubscriptionNumber == subscriptionNumber);

                if (subscriber == null)
                    return NotFound(new { message = "Prenumerant hittades inte." });

                // Uppdatera kontaktuppgifter
                subscriber.PhoneNumber = dto.PhoneNumber;
                subscriber.DeliveryAddress = dto.DeliveryAddress;
                subscriber.PostalCode = dto.PostalCode;
                subscriber.City = dto.City;

                await _context.SaveChangesAsync();

                return NoContent();
            }
        [HttpGet("export/xml")]
        public async Task<IActionResult> ExportToXml()
        {
            var subscribers = await _context.Subscribers.ToListAsync();

            var list = new SubscriberXmlList
            {
                Items = subscribers.Select(s => s.ToXmlDto()).ToList()
            };

            var serializer = new XmlSerializer(typeof(SubscriberXmlList));

            await using var ms = new MemoryStream();
            serializer.Serialize(ms, list);
            ms.Position = 0;

            return File(ms.ToArray(), "application/xml", "subscribers.xml");
        }
    }
}
