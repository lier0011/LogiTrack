namespace LogiTrack;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LogiTrack.Models;
public class LogiTrackContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=logitrack.db");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Required for Identity tables
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

    public async Task SeedRolesAndUsersAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        string[] roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Assign the user to the Admin role (change email/role as needed)
        var user = await userManager.FindByEmailAsync("test@example.com");
        if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}