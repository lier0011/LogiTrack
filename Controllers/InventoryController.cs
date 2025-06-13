namespace LogiTrack.Controllers;

using LogiTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class InventoryController : ControllerBase
{
    private readonly LogiTrackContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<OrderController> _logger;
    private const string cacheKey = "inventory_items";
    public InventoryController(LogiTrackContext context, IMemoryCache cache, ILogger<OrderController> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    // GET: /api/inventory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAll()
    {
        if (!_cache.TryGetValue(cacheKey, out List<InventoryItem>? items))
        {
            items = await _context.InventoryItems.AsNoTracking().ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };
            _cache.Set(cacheKey, items, cacheEntryOptions);
        } else {
            _logger.LogInformation("[{dateTime}] Cache hit for inventory items", DateTime.UtcNow);
        }
        return Ok(items);
    }

    private void InvalidateCache()
    {
        _cache.Remove(cacheKey);
    }

    // POST: /api/inventory
    [HttpPost]
    public async Task<ActionResult<InventoryItem>> AddItem([FromBody] InventoryItem item)
    {
        try
        {
            await _context.InventoryItems.AddAsync(item);
            await _context.SaveChangesAsync();
            InvalidateCache();
            return CreatedAtAction(nameof(GetItemById), new { id = item.ItemId }, item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing AddItem request for inventory item: {item}", item);
            return BadRequest(new { message = $"Error processing request: {ex.Message}" });
        }
    }

    // DELETE: /api/inventory/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        bool exists = await _context.InventoryItems.AsNoTracking().AnyAsync(i => i.ItemId == id);
        if (!exists)
            return NotFound(new { message = $"Not a valid inventory item" });
        var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.ItemId == id);
        if (item == null)
            return NotFound(new { message = $"Inventory item was deleted before operation" });
        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync();
        InvalidateCache();
        return NoContent();
    }

    // GET: /api/inventory/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItem>> GetItemById(int id)
    {
        var item = await _context.InventoryItems.AsNoTracking().FirstOrDefaultAsync(i => i.ItemId == id);
        if (item == null)
            return NotFound(new { message = $"Not a valid inventory item" });
        return Ok(item);
    }
}
