namespace LogiTrack.Models;
public class InventoryItem
{
    required public int ItemId { get; set; }
    required public string Name { get; set; }
    required public int Quantity { get; set; }
    required public string Location { get; set; }
    public void DisplayInfo()
    {
        Console.WriteLine($"* Item: {Name}, Quantity: {Quantity}, Location: {Location}");
    }
}