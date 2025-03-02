# SilkRoadERP-Desktop

SilkRoadERP-Desktop is a desktop client for the Silk Road ERP system. This application provides a user-friendly interface for managing sales, purchasing operations, and other business processes efficiently.

## Features
- Sales and purchase management
- Inventory tracking
- Customer and supplier management
- Reporting and analytics
- Secure authentication and role-based access control

## Technologies Used
- **.NET (WPF/WinForms)** for the desktop UI
- **Entity Framework Core** for database interaction
- **MediatR** for implementing the Mediator pattern
- **Dapper** for efficient database queries
- **AutoMapper** for object mapping
- **SQLite/MS SQL Server** as the database

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/Nieze-BenMansour/SilkRoadERP-Desktop.git
   ```
2. Navigate to the project directory:
   ```sh
   cd SilkRoadERP-Desktop
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Build the project:
   ```sh
   dotnet build
   ```
5. Run the application:
   ```sh
   dotnet run
   ```

## Configuration
- Update the `appsettings.json` file with the appropriate database connection string.
- Ensure the necessary database migrations are applied:
  ```sh
  dotnet ef database update
  ```

## Contribution
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch: `git checkout -b feature-branch`.
3. Commit your changes: `git commit -m "Add new feature"`.
4. Push to the branch: `git push origin feature-branch`.
5. Open a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
For any questions or issues, feel free to reach out via GitHub Issues or contact **Nieze Ben Mansour**.
