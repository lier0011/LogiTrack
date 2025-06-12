namespace LogiTrack.Models;
public class Order
{
    required public int OrderId { get; set; }
    required public string CustomerName { get; set; }
    required public DateTime DatePlaced { get; set; }
    public List<InventoryItem>? Items { get; set; }
    public void AddItem(InventoryItem item)
    {
        if (Items == null)
        {
            Items = new List<InventoryItem>();
        }
        Items.Add(item);
    }
    public void RemoveItem(int itemId)
    {
        if (Items != null)
        {
            Items.RemoveAll(item => item.ItemId == itemId);
        }
    }
    public void GetOrderSummary()
    {
        int count = (Items ?? Enumerable.Empty<InventoryItem>()).Count();
        Console.WriteLine($"Order #{OrderId} for {CustomerName}, Items: {count}, Date: {DatePlaced}");
        if (Items != null && count > 0)
        {
            Console.WriteLine("Items in Order:");
            foreach (var item in Items)
            {
                item.DisplayInfo();
            }
        }
        else
        {
            Console.WriteLine("No items in this order.");
        }
    }
}