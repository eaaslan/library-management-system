# Library Management System

A robust ASP.NET Core MVC application for managing library resources, books, and user rentals.

## Features

### User Management
- **User Authentication**: Secure login and registration system
- **Role-Based Authorization**: Different access levels for Admin and User roles
- **User Profile**: Users can view their rental history and current rentals

### Book Management
- **Book Catalog**: Complete listing of all available books
- **Advanced Filtering**: Filter books by title and category
- **Book Details**: Comprehensive information about each book including:
  - ISBN
  - Title
  - Author
  - Description
  - Category
  - Availability status
  - Image URL

### Admin Features
- **Book Management**:
  - Add new books
  - Update existing books
  - Delete books (soft delete)
  - Manage book categories
- **Category Management**: Organize books by categories

### User Features
- **Book Rental**: Users can rent available books
- **Rental History**: Track rental dates and return status
- **Book Search**: Easy-to-use search and filter functionality

## Technical Details

### Built With
- ASP.NET Core MVC (.NET 7.0)
- Entity Framework Core
- PostgreSQL Database
- Identity Framework for Authentication
- Bootstrap 5 for UI

### Database Schema
- **Books**: Stores book information (ISBN, Title, Author, etc.)
- **Categories**: Manages book categories
- **Rentals**: Tracks book rentals
- **Users**: Manages user information
- **Roles**: Handles role-based authorization

### Key Features Implementation
1. **Entity Framework Core**:
   - Code-first approach
   - Migration management
   - Relationship handling

2. **Repository Pattern**:
   - IBookService interface
   - Separation of concerns
   - Clean architecture principles

3. **Authentication & Authorization**:
   - ASP.NET Core Identity
   - Role-based access control
   - Secure user management

4. **Data Validation**:
   - Model validation
   - Client-side validation
   - Server-side validation

## Getting Started

### Prerequisites
- .NET 7.0 SDK
- PostgreSQL
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repository
```bash
git clone [repository-url]
```

2. Update the connection string in `appsettings.json`
```json
"ConnectionStrings": {
    "DefaultConnection": "Your-PostgreSQL-Connection-String"
}
```

3. Run Entity Framework migrations
```bash
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

### Initial Setup
- The system comes with pre-seeded:
  - Book categories
  - Admin user
  - Sample books

## Usage

### Admin Operations
1. **Managing Books**:
   - Navigate to Books section
   - Use Add/Update/Delete functions
   - Manage book details and availability

2. **Category Management**:
   - View and manage book categories
   - Associate books with categories

### User Operations
1. **Renting Books**:
   - Browse available books
   - Use filters to find specific books
   - Click "Rent" to borrow a book

2. **Viewing Rentals**:
   - Check rental history
   - View current rentals
   - Track return dates

## Contributing
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License
This project is licensed under the MIT License - see the LICENSE file for details