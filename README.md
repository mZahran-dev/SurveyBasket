# SurveyBasket API

A comprehensive ASP.NET Core Web API for managing surveys and polls with user authentication, voting capabilities, and real-time poll management.

## 🚀 Features

- **Authentication & Authorization**
  - JWT-based authentication
  - Refresh token support
  - Token revocation
  - ASP.NET Core Identity integration

- **Poll Management**
  - Create, update, and delete polls
  - Publish/unpublish polls
  - Schedule polls with start and end dates
  - View current active polls

- **Question & Answer Management**
  - Add questions to polls
  - Multiple answer options per question
  - Toggle question active status
  - Dynamic answer management

- **Voting System**
  - Submit votes for active polls
  - One vote per user per poll
  - Vote validation and duplicate prevention
  - Track submission timestamps

## 🛠️ Technology Stack

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **ASP.NET Core Identity** for user management
- **JWT Bearer Authentication**
- **FluentValidation** for request validation
- **Mapster** for object mapping

## 📋 Prerequisites

- .NET 9.0 SDK or later
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code (optional)

## 🔧 Installation

1. **Clone the repository**
```bash
   git clone <repository-url>
   cd SurveyBasket
```

2. **Update connection string**
   
   Edit `appsettings.json` and update the connection string:
```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=SurveyBasketDatabase;Trusted_Connection=True;Encrypt=False"
   }
```

3. **Update JWT settings**
   
   Configure JWT settings in `appsettings.json` or use User Secrets:
```json
   "Jwt": {
     "Key": "your-secret-key-here",
     "Issuer": "SurveyBasketApp",
     "Audience": "SurveyBasketApp users",
     "ExpiryMinutes": 30
   }
```

4. **Apply database migrations**
```bash
   dotnet ef database update
```

5. **Run the application**
```bash
   dotnet run
```

The API will be available at `https://localhost:7133` and `http://localhost:5299`

## 📚 API Endpoints

### Authentication
- `POST /auth` - Login
- `POST /auth/refresh` - Refresh access token
- `POST /auth/revoke-refresh-token` - Revoke refresh token

### Polls
- `GET /api/polls` - Get all polls
- `GET /api/polls/current` - Get current active polls
- `GET /api/polls/{id}` - Get poll by ID
- `POST /api/polls` - Create new poll
- `PUT /api/polls/{id}` - Update poll
- `DELETE /api/polls/{id}` - Delete poll
- `PUT /api/polls/{id}/togglePublish` - Toggle poll publish status

### Questions
- `GET /api/polls/{pollId}/question` - Get all questions for a poll
- `GET /api/polls/{pollId}/question/{id}` - Get specific question
- `POST /api/polls/{pollId}/question` - Add question to poll
- `PUT /api/polls/{pollId}/question/{id}` - Update question
- `PUT /api/polls/{pollId}/question/{id}/toggleStatus` - Toggle question status

### Voting
- `GET /api/polls/{pollId}/vote/votes` - Start voting (get available questions)
- `POST /api/polls/{pollId}/vote/votes` - Submit vote

## 🔐 Default Credentials

A default admin user is seeded in the database:
- **Email**: admin@example.com
- **Password**: Admin123!

## 🏗️ Project Structure
```
SurveyBasket/
├── Abstractions/          # Result pattern and error handling
├── Authentication/        # JWT provider and options
├── Contracts/            # Request/Response DTOs
├── Controllers/          # API controllers
├── Entities/            # Database entities
├── Errors/              # Error definitions
├── Persistence/         # DbContext and configurations
├── Services/            # Business logic services
└── Extensions/          # Helper extensions
```

## 🎯 Key Features Implementation

### Result Pattern
The API uses a custom Result pattern for consistent error handling:
```csharp
var result = await _pollService.GetAsync(id, cancellationToken);
return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
```

### Audit Trail
Entities inherit from `AuditableEntity` to track:
- Created by (user ID)
- Created on (timestamp)
- Updated by (user ID)
- Updated on (timestamp)

### Validation
FluentValidation is used for request validation with custom validators for each request type.

## 🚧 Work in Progress

This project is currently under active development. Upcoming features:
- Role-based authorization
- Poll results and analytics
- Email notifications
- API documentation (Swagger/OpenAPI)
- Unit and integration tests


## 🤝 Contributing

This project is still in development. Contribution guidelines will be established once the initial version is complete.

---

**Note**: This is a work-in-progress project. Some features may be incomplete or subject to change.
