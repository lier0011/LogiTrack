namespace LogiTrack.Controllers;

using Microsoft.AspNetCore.Mvc;
using LogiTrack.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly LogiTrackContext _context;
    public InventoryController(LogiTrackContext context)
    {
        _context = context;
    }

    // GET: /api/inventory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAll()
    {
        var items = await _context.InventoryItems.ToListAsync();
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
        return NoContent();
    }
}
