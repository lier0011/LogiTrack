using LogiTrack;
using LogiTrack.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext for dependency injection
builder.Services.AddDbContext<LogiTrackContext>();
// Register controllers with JSON options to ignore cycles
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register Swagger generator for Swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map controller endpoints
app.MapControllers();

app.Run();

/*
// I commented out the code below to avoid confusion, as it is not part of the main program flow.

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

                var order2 = new Order
                {
                    CustomerName = "ManUtd Ltd",
                    DatePlaced = DateTime.Now
                };

                // Add an inventory item to the order
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

                context.Orders.Add(order2);
                context.SaveChanges();
            }

            // Retrieve and display the order
            var existingOrder = context.Orders
                .Include(o => o.Items) // Include related items
                .ToList();
            Console.WriteLine(existingOrder.Count() + " orders found.");
            foreach (var order in existingOrder)
            {
                order.GetOrderSummary();
            }
        }
    }
}*/