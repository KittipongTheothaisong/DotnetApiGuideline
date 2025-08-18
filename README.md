# .NET API Clean Architecture

This project demonstrates a .NET 9 Web API following Clean Architecture principles with Entity Framework Core using an in-memory database.

## Architecture Overview

The solution is organized into the following layers:

### 1. Domain Layer (`src/Domain`)

- **Entities**: Core business entities
- Contains the `WeatherForecast` entity with business logic

### 2. Application Layer (`src/Application`)

- **Interfaces**: Contracts for repositories and services
- **DTOs**: Data Transfer Objects for API communication
- **Services**: Business logic and use cases
- No dependencies on external concerns

### 3. Infrastructure Layer (`src/Infrastructure`)

- **Data**: Entity Framework DbContext
- **Repositories**: Data access implementations
- **DependencyInjection**: Service registration
- Implements interfaces defined in the Application layer

### 4. Presentation Layer (`src/Presentation`)

- **Controllers**: Web API controllers
- Handles HTTP requests and responses
- Depends only on Application layer interfaces

## Features

- **Clean Architecture**: Proper separation of concerns
- **Entity Framework Core**: With in-memory database for development
- **Repository Pattern**: Abstracted data access
- **Service Layer**: Business logic encapsulation
- **Dependency Injection**: Loose coupling between layers
- **DTOs**: Separate models for API contracts

## API Endpoints

- `GET /api/weatherforecast` - Get all weather forecasts
- `GET /api/weatherforecast/{id}` - Get weather forecast by ID
- `POST /api/weatherforecast` - Create new weather forecast
- `PUT /api/weatherforecast/{id}` - Update weather forecast
- `DELETE /api/weatherforecast/{id}` - Delete weather forecast
- `POST /api/weatherforecast/generate?count=5` - Generate random forecasts

## Running the Application

1. Install dependencies:

   ```bash
   dotnet restore
   ```

2. Run the application:

   ```bash
   dotnet run
   ```

3. The API will be available at `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
src/
├── Domain/
│   └── Entities/
│       └── WeatherForecast.cs
├── Application/
│   ├── DTOs/
│   │   ├── WeatherForecastDto.cs
│   │   └── CreateWeatherForecastDto.cs
│   ├── Interfaces/
│   │   ├── IWeatherForecastRepository.cs
│   │   └── IWeatherForecastService.cs
│   └── Services/
│       └── WeatherForecastService.cs
├── Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Repositories/
│   │   └── WeatherForecastRepository.cs
│   └── DependencyInjection.cs
└── Presentation/
    └── Controllers/
        └── WeatherForecastController.cs
```

## Benefits of This Architecture

1. **Testability**: Each layer can be unit tested in isolation
2. **Maintainability**: Clear separation makes code easier to understand and modify
3. **Flexibility**: Easy to swap implementations (e.g., change from in-memory to SQL Server)
4. **Scalability**: Structure supports growth and additional features
5. **Independence**: Inner layers don't depend on outer layers
