# GOOD HAMBURGER API

## Description

The GOOD HAMBURGER API is an ASP.NET Core application that manages orders and menu items for a hamburger restaurant. The API allows the creation, reading, and listing of orders and menu items, including extra items and sandwiches.



## Project Structure

- **Controllers**
  - `OrderController.cs`: Controller responsible for managing orders.
  - `MenuItemController.cs`: Controller responsible for managing menu items.
    

- **Model**
  - `MenuItemModel.cs`: Model representing a menu item.
  - `OrderRequestModel.cs`: Model representing an order.

- **Services**
  - `IOrderService.cs`: Interface for the order service.
  - `OrderService.cs`: Implementation of the order service.
  - `IMenuItem.cs`: Interface for the menu item service.
  - `MenuItemService.cs`: Implementation of the menu item service.

- **Data**
  - `AppDBContext.cs`: Database context.
  - `SeedData.cs`: Class for initializing data in the database.





