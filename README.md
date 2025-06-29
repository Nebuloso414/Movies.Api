# Movies API

A robust RESTful API for managing movies with authentication, caching, versioning, and more. Built with ASP.NET Core 8.0, this solution provides a simple but elegant backend for movie management with role-based authorization.

## Features

- **RESTful API Design**: Standard CRUD operations for movie resources
- **API Versioning**: Media-type based versioning for API evolution
- **Authentication & Authorization**:
  - JWT Bearer authentication
  - Role-based access control with admin and trusted member policies
  - API key authentication for service-to-service communication
- **Output Caching**: Policy-based response caching with tag invalidation
- **Data Validation**: Request validation with error mapping
- **Pagination**: Support for paged requests and responses
- **Health Checks**: Database health monitoring
- **API Documentation**: Swagger/OpenAPI support with version-specific documentation
- **SDK Support**: Includes a client SDK for easy API consumption

## Solution Structure

The solution is organized into multiple projects:

- **Movies.Api**: Main ASP.NET Core 8.0 web API project
  - Controllers: API endpoints for movie resources
  - Auth: Authentication/authorization configuration
  - Swagger: API documentation setup
  - Health: Health check implementations
- **Movies.Application**: Core business logic and service implementations
  - Services: Movie management services
  - Database: Data access using Dapper with PostgreSQL
  - Validation: FluentValidation implementation
- **Movies.Contracts**: Data transfer objects (DTOs) shared between API and clients
  - Requests: Request models for API operations
  - Responses: Response models for API operations
- **Movies.Api.Sdk**: Client SDK for API consumption
  - Refit-based API client
  - Endpoint definitions
- **Movies.Api.Sdk.Consumer**: Example consumer application using the SDK

## API Endpoints

- **GET `/api/movies`**: Get all movies (supports filtering, sorting, pagination)
- **GET `/api/movies/{idOrSlug}`**: Get a specific movie by ID or slug
- **POST `/api/movies`**: Create a new movie (requires trusted member role)
- **PUT `/api/movies/{id}`**: Update an existing movie (requires trusted member role)
- **DELETE `/api/movies/{id}`**: Delete a movie (requires admin role)
- **PUT `/api/movies/{id}/ratings`**: Rate a movie
- **DELETE `/api/movies/{id}/ratings`**: Delete a movie rating
- **GET `/api/ratings/me`**: Get ratings for the current user

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL database
- Docker (optional)

## Setup and Configuration

### Local Development

1. Clone the repository
    ```shell
    git clone https://github.com/yourusername/movies-api.git cd movies-api
    ```

2. Configure JWT settings in `appsettings.json` or user secrets
    ```json
    {
	    "Jwt": {
		    "Key": "your-secret-key-with-at-least-32-characters",
		    "Issuer": "movies-api",
		    "Audience": "movies-client"
	    },
	    "ApiKey": "your-api-key-for-service-access"
    }
    ```

3. Docker Compose Configuration

    The solution includes a `docker-compose.yml` file for easy local development with PostgreSQL. This configuration allows you to quickly spin up a database instance without installing PostgreSQL directly on your machine.

    #### PostgreSQL Database Configuration

    Configure the environment variables in the **docker-compose.yml** file to your liking:
    ```yaml
    environment:
      - POSTGRES_USER=movies_dbuser     # Database username
      - POSTGRES_PASSWORD=P@ssw0rd1     # Database password
      - POSTGRES_DB=movies              # Database name
    ```

    #### Running with Docker Compose

    Navigate to the directory containing docker-compose.yml
    ```shell
    cd Movies.Application
    ```
    Start the database container
    ```shell
    docker-compose up -d
    ```

4. Configure the database connection in `appsettings.json` or user secrets
    ```json
    {
	    "Database": {
		    "ConnectionString": "Host=localhost;Port=5432;Database=movies;Username=postgres;Password=yourpassword;"
	    }
    }
    ```

5. Run the application
    ```shell
    dotnet run --project Movies.Api
    ```

6. Access the API at `https://localhost:5001` (or the configured port)
    - Swagger UI is available at `/swagger`
    - Health check endpoint is available at `/_health`


## Authentication

The API uses JWT Bearer authentication. To access protected endpoints:

1. Obtain a JWT token by calling the "token" endpoint in the **IdentityApi** API
2. Include the token in the Authorization header:
```Authorization: Bearer your-jwt-token```

For service-to-service communication, use the API key header: ```x-api-key: your-api-key```


## Using the SDK

The Movies.Api.Sdk project provides a client SDK for easy integration:
```csharp
// Register services 
services.AddRefitClient<IMoviesApi>() .ConfigureHttpClient(client => { client.BaseAddress = new Uri("https://api.example.com"); });

// Inject and use the client 
public class MyService 
{ 
    private readonly IMoviesApi _moviesApi;

    public MyService(IMoviesApi moviesApi)
    {
        _moviesApi = moviesApi;
    }

    public async Task DoSomethingAsync()
    {
        var movies = await _moviesApi.GetMoviesAsync(new GetAllMoviesRequest());
        // Use the movies data
    }
}
```

## Postman Collection

The repository includes a comprehensive Postman collection that allows you to quickly test all API endpoints without writing any code. The collection is organized into logical folders for different resource types.

### Importing the Collection

1. Download [Postman](https://www.postman.com/downloads/)
2. In Postman, click "Import" > "File" and select `Helpers/Movies.Api.postman_collection.json`
3. The collection will appear in your Postman workspace

### Collection Structure

The collection is organized into the following folders:

- **Identity**: Authentication-related requests
  - Token Generator: Creates a JWT token for testing authenticated endpoints
  
- **Movies**: Movie resource management
  - Create movie: POST request to add a new movie
  - Get movie: GET request to retrieve a specific movie by ID or slug
  - Get all movies: GET request to retrieve a paginated list of movies
  - Update movie: PUT request to modify an existing movie
  - Delete movie: DELETE request to remove a movie
  
- **Ratings**: Movie rating operations
  - Rate movie: PUT request to rate a specific movie
  - Get my ratings: GET request to retrieve the current user's ratings
  - Delete rating: DELETE request to remove a rating from a movie
  
- **HealthCheck**: Simple endpoint to verify API health status

### Authentication Setup

The Movies and Ratings folders have Bearer token authentication pre-configured. To use these requests:

1. First run the "Token Generator" request in the Identity folder (ensure you have the Identity API running)
2. Copy the JWT token from the response
3. The token is automatically applied to all requests in the Movies and Ratings folders

### Using Request Variables

When working with the collection:

1. Replace `{id}` or `{idOrSlug}` in URLs with actual values
2. The base URL is set to `https://localhost:5001` - update this if your API runs on a different port

