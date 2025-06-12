using LogiTrack.Models;

Order order = new Order
{
    OrderId = 123,
    CustomerName = "John Doe",
    DatePlaced = DateTime.Now
};

order.AddItem(new InventoryItem
{
    ItemId = 1,
    Name = "Smartphone",
    Quantity = 10,
    Location = "Manchester"
});
order.AddItem(new InventoryItem
{
    ItemId = 2,
    Name = "Monitor",
    Quantity = 5,
    Location = "Tasikmalaya"
});
order.GetOrderSummary();