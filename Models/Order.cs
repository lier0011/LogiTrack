namespace LogiTrack.Models;
using System.ComponentModel.DataAnnotations;
public class Order
{
    [Key]
    public int OrderId { get; set; }
    [Required]
    required public string CustomerName { get; set; }
    [Required]
    required public DateTime DatePlaced { get; set; } = DateTime.Now;
    public List<InventoryItem> Items { get; set; }
    public Order()
    {
        Items = new List<InventoryItem>();
    }
    public void AddItem(InventoryItem item)
    {
        Items.Add(item);
    }
    public void RemoveItem(int itemId)
    {
        Items.RemoveAll(item => item.ItemId == itemId);
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