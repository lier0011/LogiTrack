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
        }
        return Ok(items);
    }

    // POST: /api/inventory
    [HttpPost]
    public async Task<ActionResult<InventoryItem>> AddItem([FromBody] InventoryItem item)
    {
        try
        {
            await _context.InventoryItems.AddAsync(item);
            await _context.SaveChangesAsync();
            // Suggestion 4: Remove cache after data change
            _cache.Remove(cacheKey);
            return CreatedAtAction(nameof(GetAll), new { id = item.ItemId }, item);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error processing request: {ex.Message}" });
        }
    }

    // DELETE: /api/inventory/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null)
            return NotFound(new { message = $"Not a valid inventory item" });
        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync();
        // Suggestion 4: Remove cache after data change
        _cache.Remove(cacheKey);
        return NoContent();
    }
}
