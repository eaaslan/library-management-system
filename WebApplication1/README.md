# Library Management System

A modern library management system built with ASP.NET Core MVC that allows users to browse, rent, and manage books.

## Features

- User Authentication & Authorization
  - User registration and login
  - Role-based access control (Admin and User roles)
  - Secure password handling

- Book Management
  - Browse books with pagination (15 books per page)
  - Filter books by title and category
  - Add, update, and soft delete books (Admin only)
  - Book details include ISBN, title, author, description, and category

- Category Management
  - Predefined book categories
  - Filter books by category
  - Category-based organization

- Rental System
  - Users can rent available books
  - Automatic return date calculation
  - Track rental history

## Technologies

- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL
- Docker
- Bootstrap
- Identity Framework for Authentication

## Getting Started

1. Prerequisites:
   - .NET 6.0 SDK
   - Docker Desktop
   - Visual Studio 2022 or VS Code

2. Clone the repository:
   ```bash
   git clone https://github.com/eaaslan/library-management-system.git
   ```

3. Start the PostgreSQL database:
   ```bash
   docker-compose up -d
   ```

4. Update the database:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

6. Access the application at `http://localhost:5143`

## Default Admin Account

- Email: admin@library.com
- Password: Admin123!

## License

This project is licensed under the MIT License - see the LICENSE file for details. 