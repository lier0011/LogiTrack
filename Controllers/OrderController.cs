using Microsoft.AspNetCore.Mvc;
using LogiTrack.Models;
using LogiTrack;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly LogiTrackContext _context;
    public OrderController(LogiTrackContext context)
    {
        _context = context;
    }

    // GET: /api/order
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _context.Orders.Include(o => o.Items).ToListAsync();
        return Ok(orders);
    }

    // GET: /api/order/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
            return NotFound(new { message = $"Not a valid order" });
        return Ok(order);
    }

    // POST: /api/order
    [HttpPost]
    public async Task<ActionResult<Order>> AddOrder([FromBody] Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = order.OrderId }, order);
    }

    // DELETE: /api/order/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound(new { message = $"Not a valid order" });
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
