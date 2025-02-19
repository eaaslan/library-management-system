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
![Admin Book View](https://github.com/user-attachments/assets/458be9c3-be4f-4776-a9e1-dfa60ef98881)

### 👥 Admin User Management
![Admin User Management View](https://github.com/user-attachments/assets/4099c4cb-5d40-4d59-9924-0154150d0f11)

### 📋 Admin Rental History
![Admin Rental History View](https://github.com/user-attachments/assets/5ee3ac24-9bb7-4779-b8d0-e3b73ed743fb)

### 📊 Admin Dashboard
![Admin Dashboard View](https://github.com/user-attachments/assets/e93df45b-fc08-49dd-bce4-ec1f7835637b)

### 🔒 Public View
![Unauthenticated User View](https://github.com/user-attachments/assets/96cd8f38-1ff1-40ee-aaf5-2d9bdd15a496)

### 📖 User Book View
![User Book View](https://github.com/user-attachments/assets/f9b39a77-34fb-456e-ad9f-a8e9f60d2943)

### 📅 User Rental History
![User Rental History View](https://github.com/user-attachments/assets/3946e162-ca3d-43f9-bb7a-d371ed46f6cd)

## Contributing
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License
[MIT License](LICENSE)
