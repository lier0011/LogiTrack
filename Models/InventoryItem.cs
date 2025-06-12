namespace LogiTrack.Models;
using System.ComponentModel.DataAnnotations;
public class InventoryItem
{
    [Key]
    public int ItemId { get; set; }
    [Required]
    required public string Name { get; set; }
    [Required]
    required public int Quantity { get; set; }
    [Required]
    required public string Location { get; set; }
    public void DisplayInfo()
    {
        Console.WriteLine($"* Item: {Name}, Quantity: {Quantity}, Location: {Location}");
    }
}