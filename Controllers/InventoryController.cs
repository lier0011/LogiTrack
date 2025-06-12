namespace LogiTrack.Controllers;

using Microsoft.AspNetCore.Mvc;
using LogiTrack.Models;

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
    public ActionResult<IEnumerable<InventoryItem>> GetAll()
    {
        return Ok(_context.InventoryItems.ToList());
    }

    // POST: /api/inventory
    [HttpPost]
    public ActionResult<InventoryItem> AddItem([FromBody] InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = item.ItemId }, item);
    }

    // DELETE: /api/inventory/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteItem(int id)
    {
        var item = _context.InventoryItems.Find(id);
        if (item == null)
            return NotFound();
        _context.InventoryItems.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
