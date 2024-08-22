using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Data.SeedData;
using GOOD_HAMBURGER.Services.MenuItem;
using GOOD_HAMBURGER.Services.OrderItem;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container and configure the application
builder.Services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase("InMemoryAppDb"));
builder.Services.AddScoped<IMenuItem, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Add JsonStringEnumConverter to the JSON options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
using (var scope = app.Services.CreateScope())
{
    // Get the instance of AppDBContext in our services layer 
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
    dbContext.Database.EnsureCreated();
    // Seed the database 
    await DbInitializer.InitializeAsync(dbContext, orderService);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
