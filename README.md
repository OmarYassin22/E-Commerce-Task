# E-Commerce Task

## Overview
This project utilizes several technologies and frameworks to build a robust e-commerce application. Here is an overview of the key components used:

- **.NET Framework 4.8** with Dependency Injection
- **Entity Framework 6.5.1** for data access
- **ASP.NET Identity** for authentication and authorization
- **NLog** for logging information and errors
- **Generic Repository Pattern** for business logic

## Technologies Used

### .NET Framework 4.8 with Dependency Injection
The project is built using .NET Framework 4.8 and employs Dependency Injection to manage dependencies. This allows for more maintainable and testable code.

### Entity Framework 6.5.1
Entity Framework 6.5.1 is used as the ORM (Object-Relational Mapper) for data access. It simplifies data manipulation and management.

### ASP.NET Identity
ASP.NET Identity is used to handle authentication and authorization within the application, ensuring secure access to resources.

### NLog
NLog is used for logging information and errors. It helps in tracking application behavior and debugging issues effectively.

### Generic Repository Pattern
The business logic is implemented using the Generic Repository Pattern, which promotes a clean separation of concerns and code reusability.

## Models Used
- **User:** Represents the users of the application.
- **Product:** Represents the products available in the e-commerce platform.
- **Order:** Represents the orders placed by users.
- **OrderDetail:** Represents the details of each order.

## Configuration
Ensure the following packages are installed as per the `packages.config` and `E-Commerce.csproj` file:
- EntityFramework 6.5.1
- Microsoft.Extensions.DependencyInjection.Abstractions 2.1.0
- NLog
- FluentValidation 11.11.0
- Mapster 5.3.0
- Unity 5.11.10

For more information, visit the [source files](https://github.com/OmarYassin22/E-Commerce-Task).
