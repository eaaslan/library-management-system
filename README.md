# Library Management System

A robust ASP.NET Core MVC application for managing library resources, books, and user rentals.

## Features

### User Management
- **Role-Based Authorization**: 
  - Admin: Full system access
  - Library Staff: Book and rental management
  - Users: Book browsing and rental
- **User Authentication**: Secure login and registration system
- **User Verification System**: Active/Inactive status management
- **User Profile**: Users can view their rental history and current rentals
- **Notifications**: 
  - Admin receives notifications for:
    - New user registrations
    - Book return deadlines
    - Overdue rentals
    - Low book stock alerts

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
  - Publication year
  - Image URL
- **Pagination**: 
  - Efficient browsing of large book collections
  - Configurable items per page
  - Sort and filter capabilities

### Rental System
- **Book Rental**: Users can rent available books
- **Return Tracking**: Monitor return dates and status
- **Rental History**: Complete rental transaction history
- **Active Rental Monitoring**: Track currently rented books


## Technical Setup

### Prerequisites
- .NET SDK 8.0.405
- Docker
- Docker Compose
- PostgreSQL (via Docker)

### Built With
- ASP.NET Core MVC (.NET 8.0)
- Entity Framework Core
- PostgreSQL Database
- Identity Framework for Authentication
- Bootstrap 5 for UI

### Database Configuration
Development database runs on Docker with these credentials:
- Host: localhost
- Port: 5433
- Database: postgres
- Username: postgres
- Password: mysecretpassword

### Initial Data
The system comes pre-seeded with:
- Categories (imported from CSV)
- Books (imported from CSV)
- Default users:
  - Admin (admin@example.com / Admin123!)
  - 3 Library Staff members (staff1@example.com through staff3@example.com / Staff123!)
  - 10 Regular users (user1@example.com through user10@example.com / User123!)
    - First 5 users are active and verified
- Sample rental history

## Getting Started

1. Clone the repository
   ```bash
   git clone https://github.com/eaaslan/library-management-system.git
   ```

2. Start the PostgreSQL container
   ```bash
   docker-compose up -d
   ```

3. Run the application
   ```bash
   dotnet run
   ```

The application will automatically:
1. Create and migrate the database
2. Import categories from CSV
3. Import books from CSV
4. Create default users and roles
5. Generate sample rental data

## Project Structure
```
WebApplication1/
├── Data/
│   ├── CSV/
│   │   ├── categories.csv
│   │   └── books.csv
│   └── SeedData.cs
├── Models/
├── Controllers/
├── docker-compose.yml
└── appsettings.json
```

## Development Notes
- CSV files in Data/CSV/ are automatically imported on first run
- Database seeding happens in order: Categories → Books → Users → Rentals
- Development and Production environments use different connection strings
- Docker container includes health checks for PostgreSQL

## Default Credentials

### Admin
- Email: admin@example.com
- Password: Admin123!

### Library Staff
- Email: staff1@example.com (through staff3@example.com)
- Password: Staff123!

### Regular Users
- Email: user1@example.com (through user10@example.com)
- Password: User123!
- Note: Only users 1-5 are active and verified

## Screenshots

### 📚 Admin Book Management
![admin_books_1](https://github.com/user-attachments/assets/98439be7-4a4a-45c3-a4ef-b7b23f0a20cb)
![admin_book_update](https://github.com/user-attachments/assets/685392d8-63a1-41f9-8ef8-46ef2c577194)

### 👥 Admin User Management
![admin_users](https://github.com/user-attachments/assets/8c77779f-30bb-4853-b2b1-92bfad9a582d)

### 📋 Admin Rental History
![admin_rentals](https://github.com/user-attachments/assets/5357780f-441b-4b54-8e77-3795a6956312)

### 📊 Admin Dashboard
![admin_dashboard_1](https://github.com/user-attachments/assets/b6361e04-9900-4555-ac08-ef4f2dfe041d)
![admin_dashboard_2_notification_rental](https://github.com/user-attachments/assets/724c3280-5474-4b00-beec-08860ffd2a06)

### 🔒 Public View
![unauth_book](https://github.com/user-attachments/assets/d5962dc2-4b24-4eba-845c-0ffe22b25374)
![login](https://github.com/user-attachments/assets/e320e946-9f71-4569-948b-cda3af756d7a)
![register](https://github.com/user-attachments/assets/fdf4effc-82a7-4a48-a268-7772205fbd2d)

### 📖 User Book View
![user_bookview_1](https://github.com/user-attachments/assets/757735cb-8168-46c2-ba21-9ed40d29e743)
![user_book_view_2](https://github.com/user-attachments/assets/35707d28-1606-4d7b-ab69-93d5337e7a6c)

### 📅 User Rental History
![user_rental_view](https://github.com/user-attachments/assets/fd0e3b67-6bb0-4090-b16c-3cf5a46816df)

## Contributing
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License
[MIT License](LICENSE)
