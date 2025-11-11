using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Subscribersystem_API.Data;
using Subscribersystem_API.Models;

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
    }
}
