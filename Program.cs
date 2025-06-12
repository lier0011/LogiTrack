using LogiTrack;
using LogiTrack.Models;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new LogiTrackContext())
        {
            // Add order if none exist
            if (!context.Orders.Any())
            {
                var order = new Order
                {
                    CustomerName = "Acme Corp",
                    DatePlaced = DateTime.Now
                };

                // Add an inventory item to the order
                order.AddItem(new InventoryItem
                {
                    Name = "Forklift",
                    Quantity = 5,
                    Location = "Warehouse B"
                });
                order.AddItem(new InventoryItem
                {
                    Name = "Pallet Jack",
                    Quantity = 12,
                    Location = "Warehouse A"
                });

                context.Orders.Add(order);
                context.SaveChanges();
            }

            // Retrieve and display the order
            var existingOrder = context.Orders
                .Include(o => o.Items) // Include related items
                .FirstOrDefault();
            if (existingOrder != null)
            {
                existingOrder.GetOrderSummary();
            }
        }
    }
}