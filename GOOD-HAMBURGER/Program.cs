using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Data.SeedData;
using GOOD_HAMBURGER.Services;
using GOOD_HAMBURGER.Services.MenuItem;
using GOOD_HAMBURGER.Services.OrderItem;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase("InMemoryAppDb"));

builder.Services.AddScoped<IMenuItem, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>(); // Corrigido para usar OrderService

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Make sure table is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
    dbContext.Database.EnsureCreated();
    DbInitializer.Initialize(dbContext, orderService);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
