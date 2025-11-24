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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscriber>>> GetAll()
        {
            var subs = await _context.Subscribers.ToListAsync();
            return Ok(subs);
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
        [HttpPost("import/xml")]
        public async Task<IActionResult> ImportFromXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Ingen fil uppladdad." });

            SubscriberXmlList? list;
            var serializer = new XmlSerializer(typeof(SubscriberXmlList));

            await using (var stream = file.OpenReadStream())
            {
                try
                {
                    list = serializer.Deserialize(stream) as SubscriberXmlList;
                }
                catch
                {
                    return BadRequest(new { message = "Kunde inte läsa XML-filen." });
                }
            }

            if (list == null || list.Items.Count == 0)
                return BadRequest(new { message = "XML-filen innehåller inga prenumeranter." });

            int added = 0;
            foreach (var dto in list.Items)
            {
                if (string.IsNullOrWhiteSpace(dto.SubscriptionNumber))
                    continue;

                var exists = await _context.Subscribers
                    .AnyAsync(s => s.SubscriptionNumber == dto.SubscriptionNumber);

                if (exists)
                    continue;

                var entity = dto.ToEntity();
                _context.Subscribers.Add(entity);
                added++;
            }

            if (added > 0)
                await _context.SaveChangesAsync();

            return Ok(new { message = $"Import klar. Nya prenumeranter: {added}." });
        }
        // POST: api/Subscribers
        [HttpPost]
        public async Task<ActionResult<Subscriber>> Create([FromBody] Subscriber subscriber)
        {
            if (string.IsNullOrWhiteSpace(subscriber.SubscriptionNumber))
                return BadRequest(new { message = "Prenumerationsnummer krävs." });

            var exists = await _context.Subscribers
                .AnyAsync(s => s.SubscriptionNumber == subscriber.SubscriptionNumber);

            if (exists)
                return Conflict(new { message = "Prenumerant med detta nummer finns redan." });

            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubscriber),
                new { subscriptionNumber = subscriber.SubscriptionNumber }, subscriber);
        }

        // PUT: api/Subscribers/{subscriptionNumber}
        [HttpPut("{subscriptionNumber}")]
        public async Task<IActionResult> Update(string subscriptionNumber, [FromBody] Subscriber updated)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.SubscriptionNumber == subscriptionNumber);

            if (subscriber == null)
                return NotFound(new { message = "Prenumerant hittades inte." });

            subscriber.PersonalNumber = updated.PersonalNumber;
            subscriber.FirstName = updated.FirstName;
            subscriber.LastName = updated.LastName;
            subscriber.PhoneNumber = updated.PhoneNumber;
            subscriber.DeliveryAddress = updated.DeliveryAddress;
            subscriber.PostalCode = updated.PostalCode;
            subscriber.City = updated.City;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Subscribers/{subscriptionNumber}
        [HttpDelete("{subscriptionNumber}")]
        public async Task<IActionResult> Delete(string subscriptionNumber)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.SubscriptionNumber == subscriptionNumber);

            if (subscriber == null)
                return NotFound(new { message = "Prenumerant hittades inte." });

            _context.Subscribers.Remove(subscriber);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
