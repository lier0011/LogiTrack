using LogiTrack;
using LogiTrack.Models;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new LogiTrackContext())
        {
            // Add test inventory item if none exist
            if (!context.InventoryItems.Any())
            {
                context.InventoryItems.Add(new InventoryItem
                {
                    Name = "Pallet Jack",
                    Quantity = 12,
                    Location = "Warehouse A"
                });

                context.SaveChanges();
            }

            // Retrieve and print inventory to confirm
            var items = context.InventoryItems.ToList();
            foreach (var item in items)
            {
                item.DisplayInfo(); // Should print: Item: Pallet Jack | Quantity: 12 | Location: Warehouse A
            }
        }
    }
}