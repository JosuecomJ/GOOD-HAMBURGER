## Description
The GOOD-HAMBURGER project is an ASP.NET Core application that manages menu items and orders. It consists of controllers that handle HTTP requests, services that contain business logic, models that represent database entities, and DTOs used to transfer data between application layers. The `AppDBContext` is responsible for managing the database connection and CRUD operations.


This structure is organized for clarity, making it easier to understand the project components and challenges faced during development.

### Rules:
- If the customer selects a sandwich, fries, and soft drink, then the customer will have 20%
discount.
- If the customer selects a sandwich and soft drink, then the customer will have 15% discount.
- If the customer selects a sandwich and fries, then the customer will have a 10% discount.
- Each order cannot contain more than one sandwich, fries, or soda. If two identical items are
sent, the API should return an error message displaying the reason.

### The project has:
- endpoint to list sandwiches and extras.
- endpoint to list sandwiches only.
- endpoint to list extra only.
-  endpoint to send an order and return the amount that will be charged to the
customer.
- endpoint to list all orders.
- endpoint to update an order.
- endpoint to remove an order



## Personal Challenges

### 1. Model and Variable Naming
Initially, there was uncertainty regarding the naming of the collection of items associated with an order. The model was originally named `MealItemModel`, but as the project progressed, it became evident that this name did not fully capture its purpose.

- **Solution**: The model and associated variables were refactored to `MenuItemModel`, aligning more accurately with its role in representing items on the restaurant's menu.

### 2. Database Relationship Type
The project originally used a "One-to-Many" relationship between `OrderRequestModel` and `MenuItemModel`. This configuration allowed an `OrderRequest` to contain multiple `MenuItems`, but it restricted a `MenuItem` to a single `OrderRequest`, leading to data integrity issues.

- **Solution**: The relationship was changed to a many-to-many structure, allowing a `MenuItem` to be associated with multiple `OrderRequests`. This adjustment resolved the data integrity issues.

- **New Challenge**: A key conflict arose after implementing the many-to-many relationship due to identical ID properties in both models, leading to `ArgumentException` errors.

  - **Solution**: The ID properties were renamed to clearly reference their respective objects, eliminating the key conflict and ensuring proper functionality of the many-to-many relationship.

## **References**
- [Entity Framework Core Relationships](https://learn.microsoft.com/en-us/ef/core/modeling/relationships).
- [Stack Overflow: Key Conflict Discussion](https://stackoverflow.com/questions/5648060/argumentexception-an-item-with-the-same-key-has-already-been-added)

## Project Structure: GOOD-HAMBURGER

### Directories and Files

#### 1. GOOD-HAMBURGER/Controllers

- **OrderController.cs**
  - Manages requests related to orders.
  - **Methods:**
    - `GetOrders()`: Returns all orders.
    - `GetOrderById(int id)`: Returns an order by its ID.
    - `CreateOrder(CreateOrderRequestDTO orderRequestDto)`: Creates a new order.
    - `UpdateOrder(int id, CreateOrderRequestDTO orderRequestDto)`: Updates an existing order.
    - `DeleteOrder(int id)`: Deletes an order by its ID.

- **MenuItemController.cs**
  - Manages requests related to menu items.
  - **Methods:**
    - `GETMenuItems()`: Returns all menu items.
    - `GetExtraItemsONLY()`: Returns only extra items.
    - `GetSandwichesONLY()`: Returns only sandwiches.
    - `GetMenuItemById(int id)`: Returns a menu item by its ID.
    - `AddMenuItem(CreateMenuItemDTO newItemDto)`: Adds a new item to the menu.

#### 2. GOOD-HAMBURGER/Services/MenuItem

- **MenuItemService.cs**
  - Contains business logic related to menu items.
  - **Methods:**
    - `GETMenuItems()`: Retrieves all menu items from the database.
    - `GETExtraItemsONLY()`: Retrieves only extra items from the database.
    - `GETSandwichesONLY()`: Retrieves only sandwiches from the database.
    - `GETMenuItemById(int MenuId)`: Retrieves a menu item by its ID.
    - `AddMenuItem(CreateMenuItemDTO newItemDto)`: Adds a new item to the menu.

- **IMenuItem.cs**
  - Interface defining methods for `MenuItemService`.

#### 3. GOOD-HAMBURGER/Services/OrderItem

- **OrderService.cs**
  - Contains business logic related to orders.
  - **Methods:**
    - `CalculateAndSaveOrderTotalAsync(OrderRequestModel order)`: Calculates and saves the total order amount.
    - `GetOrdersAsync()`: Retrieves all orders.
    - `GetOrderByIdAsync(int id)`: Retrieves an order by its ID.
    - `CreateOrderAsync(CreateOrderRequestDTO orderRequestDto)`: Creates a new order.
    - `UpdateOrderAsync(int id, CreateOrderRequestDTO orderRequestDto)`: Updates an existing order.
    - `DeleteOrderAsync(int id)`: Deletes an order by its ID.

- **IOrderService.cs**
  - Interface defining methods for `OrderService`.

#### 4. GOOD-HAMBURGER/Data

- **AppDBContext.cs**
  - Manages entities and their relationships in the database.

#### 5. GOOD-HAMBURGER/DTOs

- **CreateMenuItemDTO.cs**
  - Data Transfer Object (DTO) for creating a new menu item.

- **CreateOrderRequestDTO.cs**
  - Data Transfer Object (DTO) for creating a new order.

#### 6. GOOD-HAMBURGER/Model

- **MenuItemModel.cs**
  - Represents a menu item.

- **OrderRequestModel.cs**
  - Represents an order.

- **OrderMenuItem.cs**
  - Represents the relationship between an order and menu items.

#### 7. Other Files

- **Program.cs**
  - Configures and starts the application.
