# Library Management System

A robust ASP.NET Core MVC application for managing library resources, books, and user rentals.

## Features

### User Management
- **User Authentication**: Secure login and registration system
- **Role-Based Authorization**: Different access levels for Admin, Library Worker, and User roles
- **User Profile**: Users can view their rental history and current rentals
- **User Activation**: Users need to be verified by library workers before becoming active
- **User Filtering**: Advanced filtering system for administrators to manage users
  - Filter by name (first or last name)
  - Filter by email
  - Filter active/inactive users
  - Sort users by active status

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
- **Soft Delete**: Books can be marked as deleted without permanent removal

### Admin Features
- **Dashboard**: 
  - Overview of system statistics
  - Total books, users, and rentals
  - Active rentals monitoring
  - Recent rental activities
- **Book Management**:
  - Add new books
  - Update existing books
  - Delete books (soft delete)
  - Manage book categories
- **User Management**:
  - View all users
  - Update user details
  - Activate/deactivate users
  - Filter and search users
- **Rental Management**:
  - Track all rentals
  - Monitor active rentals
  - View rental history

### Library Worker Features
- **User Verification**: Ability to verify and activate new users
- **Rental Management**: Monitor and manage book rentals
- **User Management**: Access to user information and status

### User Features
- **Book Rental**: Users can rent available books
- **Rental History**: Track rental dates and return status
- **Book Search**: Easy-to-use search and filter functionality
- **Account Status**: Users can see their activation status

## Technical Details

### Built With
- ASP.NET Core MVC (.NET 8.0)
- Entity Framework Core
- PostgreSQL Database
- Identity Framework for Authentication
- Bootstrap 5 for UI

### Database Schema
- **Books**: Stores book information (ISBN, Title, Author, etc.)
- **Categories**: Manages book categories
- **Rentals**: Tracks book rentals and status
- **Users**: Manages user information and activation status
- **Roles**: Handles role-based authorization (Admin, Library Worker, User)

### Key Features Implementation
1. **Entity Framework Core**:
   - Code-first approach
   - Migration management
   - Relationship handling
   - Soft delete implementation

2. **Repository Pattern**:
   - IBookService interface
   - Separation of concerns
   - Clean architecture principles

3. **Authentication & Authorization**:
   - ASP.NET Core Identity
   - Role-based access control
   - User activation system
   - Secure user management

4. **Data Validation**:
   - Model validation
   - Client-side validation
   - Server-side validation

5. **Advanced Filtering**:
   - Dynamic query building
   - Multiple filter criteria
   - Case-insensitive search
   - Combined filters

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repository
```bash
git clone https://github.com/eaaslan/library-management-system.git
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
  - Admin user (admin@library.com / Admin123!)
  - Library workers (worker1@library.com / Worker123!)
  - Sample users (user1@test.com / User123!)
  - Sample books
  - Sample rental records

## Usage

### Admin Operations
1. **Managing Books**:
   - Navigate to Books section
   - Use Add/Update/Delete functions
   - Manage book details and availability

2. **Managing Users**:
   - View all users in the system
   - Filter users by name or email
   - Activate/deactivate users
   - Update user information

3. **Monitoring System**:
   - View dashboard statistics
   - Track rental activities
   - Monitor user activities

### Library Worker Operations
1. **User Management**:
   - Verify new users
   - Monitor user activities
   - Update user status

2. **Rental Management**:
   - Track active rentals
   - Monitor return dates
   - Handle rental issues

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
