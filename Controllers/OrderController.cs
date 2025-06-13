namespace LogiTrack.Controllers;

using LogiTrack.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly LogiTrackContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<OrderController> _logger;
    private const string cacheKey = "order_items";
    public OrderController(LogiTrackContext context, IMemoryCache cache, ILogger<OrderController> logger)
    {
        _cache = cache;
        _context = context;
        _logger = logger;
    }

    // GET: /api/order
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        if (!_cache.TryGetValue(cacheKey, out List<Order>? orders))
        {
            orders = await _context.Orders.AsNoTracking().ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };
            _cache.Set(cacheKey, orders, cacheEntryOptions);
        }
        else
        {
            _logger.LogInformation("[{dateTime}] Cache hit for order items", DateTime.UtcNow);
        }
        return Ok(orders);
    }

    // GET: /api/order/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        // Suggestion 5: Use AnyAsync for existence check before fetching
        bool exists = await _context.Orders.AsNoTracking().AnyAsync(o => o.OrderId == id);
        if (!exists)
            return NotFound(new { message = $"Not a valid order" });
        var order = await _context.Orders.Include(o => o.Items).AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id);
        return Ok(order);
    }

    // POST: /api/order
    [HttpPost]
    public async Task<ActionResult<Order>> AddOrder([FromBody] Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            InvalidateCache();
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing AddOrder request for order: {order}", order);
            return BadRequest(new { message = $"Error processing request: {ex.Message}" });
        }
    }

    // DELETE: /api/order/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        bool exists = await _context.Orders.AsNoTracking().AnyAsync(o => o.OrderId == id);
        if (!exists)
            return NotFound(new { message = $"Not a valid order" });
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
            return NotFound(new { message = $"Order was deleted before operation" });
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        InvalidateCache();
        return NoContent();
    }

    private void InvalidateCache()
    {
        _cache.Remove(cacheKey);
    }
}
