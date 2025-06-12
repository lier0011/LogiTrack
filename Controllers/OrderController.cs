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

    // GET: /api/orders
    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        return Ok(_context.Orders.Include(o => o.Items).ToList());
    }

    // GET: /api/orders/{id}
    [HttpGet("{id}")]
    public ActionResult<Order> GetOrderById(int id)
    {
        var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.OrderId == id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    // POST: /api/orders
    [HttpPost]
    public ActionResult<Order> AddOrder([FromBody] Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = order.OrderId }, order);
    }

    // DELETE: /api/orders/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound();
        _context.Orders.Remove(order);
        _context.SaveChanges();
        return NoContent();
    }
}
