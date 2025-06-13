namespace LogiTrack;

using Microsoft.EntityFrameworkCore;
using LogiTrack.Models;
public class LogiTrackContext : DbContext
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=logitrack.db");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>()
            .HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);
    }

    public void SeedSampleData()
    {
        if (!Orders.Any())
        {
            var order = new Order
            {
                CustomerName = "Acme Corp",
                DatePlaced = DateTime.Now
            };
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
            Orders.Add(order);

            var order2 = new Order
            {
                CustomerName = "ManUtd Ltd",
                DatePlaced = DateTime.Now
            };
            order2.AddItem(new InventoryItem
            {
                Name = "Boots",
                Quantity = 50,
                Location = "Warehouse Salford"
            });
            order2.AddItem(new InventoryItem
            {
                Name = "Training Kit",
                Quantity = 100,
                Location = "Warehouse Eccles"
            });
            Orders.Add(order2);
            SaveChanges();
        }
    }
}