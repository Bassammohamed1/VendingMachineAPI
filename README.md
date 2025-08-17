# Vending Machine API

This project is a **.NET Web API** that provides functionalities for managing accounts and products within a vending machine system.  
It follows a **3-Tier Architecture** (Presentation Layer, Business Logic Layer, Data Access Layer) to ensure scalability, maintainability, and separation of concerns.

## Features

- **User Management**
  - Registration
  - Login (with authentication)
  - Account management

- **Product Management**
  - Add, update, delete products
  - View available products

- **Purchase Functionalities**
  - Buy products
  - Track user balance
  - Deduct balance after purchase

## Architecture

The project uses a **3-Tier Architecture**:

- **Presentation Layer (API Layer):** Handles HTTP requests and responses.  
- **Business Logic Layer (BLL):** Contains core business rules and logic.  
- **Data Access Layer (DAL):** Manages interaction with the database using repositories.  

This separation ensures modularity and makes the system easier to extend and maintain.

## Technologies Used

- **.NET 8 Web API**
- **Entity Framework Core**
- **SQL Server**
- **C#**
- **JWT Authentication** (if used)

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/vending-machine-api.git

    Navigate to the project directory:

cd vending-machine-api

Update the database connection string in appsettings.json.

Apply migrations:

dotnet ef database update

Run the application:

    dotnet run

API Endpoints
Authentication

    POST /api/auth/register – Register a new user

    POST /api/auth/login – Login and retrieve JWT

Users

    GET /api/users/{id} – Get user by ID

    PUT /api/users/{id} – Update user details

Products

    GET /api/products – Get all products

    POST /api/products – Add a new product

    PUT /api/products/{id} – Update product

    DELETE /api/products/{id} – Delete product

Purchases

    POST /api/purchase – Buy a product

License

This project is licensed under the MIT License.
