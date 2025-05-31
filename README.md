# Complete API Server for Company & Employees DB
# ASP.NET Core Web API Project

This project is an implementation of the **ASP.NET Core Web API** concepts based on a book study with 97 commits, each reflecting different stages of development and feature implementation.

## Table of Contents
- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

## Overview
The project follows best practices in building **RESTful Web APIs** using **ASP.NET Core**. It demonstrates fundamental and advanced concepts such as dependency injection, validation, data shaping, content negotiation, and real-time communication with SignalR.

## Technologies Used
- ASP.NET Core
- Entity Framework Core
- MSSQL
- Swagger
- AutoMapper
- JWT Authentication
- C#

## Features
- CRUD operations for entities
- Data validation
- Content negotiation (JSON/XML)
- Pagination and filtering
- Dependency Injection
- Token-based Authentication
- Real-time notifications with SignalR
- Docker containerization

## Installation
1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/your-repo.git
   cd your-repo
   ```

2. Install required dependencies:

   ```bash
   dotnet restore
   ```

3. Configure the database connection in `appsettings.json`.

4. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

5. Run the application:

   ```bash
   dotnet run
   ```

## Usage
Access the API via:

```bash
https://localhost:5001/api/{resource}
```

Swagger documentation is available at:

```bash
https://localhost:5001/swagger
```

## API Endpoints
| Method | Endpoint        | Description           |
|--------|----------------|---------------------|
| GET    | /api/items     | Get all items       |
| GET    | /api/items/{id} | Get item by ID      |
| POST   | /api/items     | Create new item     |
| PUT    | /api/items/{id} | Update item by ID   |
| DELETE | /api/items/{id} | Delete item by ID   |

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
This project is licensed under the MIT License.

