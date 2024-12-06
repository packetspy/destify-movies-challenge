# Destify Movies Challenge

This repository contains a solution for the Destify Movies Challenge, implemented using .NET 6, Entity Framework Core, and Swagger. It provides a robust Web API to manage movies, actors, and ratings while adhering to SOLID principles and incorporating features such as pagination, API token validation, and search functionality.

### Inital Data

The initial dataset used to populate the database is based on the Top 250 Movies from IMDb and was sourced from the following repository: [movies-250.json.](https://github.com/toedter/movies-demo/blob/master/backend/src/main/resources/static/movie-data/movies-250.json)

### Frontend react

The frontend app was started with React + TypeScript + Vite and inspired by: [Bulletproof React](https://github.com/alan2207/bulletproof-react)

# Features

## Backend

- CRUD Operations: Manage movies, actors, and movie ratings.
- Entity Relationships:

  - Movies have multiple actors and ratings.
  - Actors can appear in multiple movies.
  - Directors can appear in multiple movies.

- Search Functionality:

  - Search movies and actors by partial names.

- Pagination:

  - Paginate results for movie and actor listings.

- Authentication:
  - API token validation for protected endpoints (Create, Update, Delete).
- Swagger Integration:
  - Automatically generated API documentation.

# Prerequisites

- .NET 6 SDK: Download here
- Git: Download here
- Docker (optional): For containerization.

# Getting Started

## Clone the Repository

```bash
git clone https://github.com/packetspy/destify-movies-challenge.git
cd destify-movies-challenge
```

### Running the Project

#### 1. Via Command Line

1. Navigate to the destify-movies-challenge directory.

2. Restore dependencies:

```bash
dotnet restore
```

3.  Run the application:

```bash
dotnet run --project MovieChallenge.Api
```

#### 2. Via Docker (Optional)

Build the Docker image:

```bash
cd solutionn
docker-compose up -d build -t destify-movies-challenge .
```

# API Documentation

Once the application is running, access the Swagger UI at:

APP: [http://localhost:5050/](http://localhost:5050/)
API: [http://localhost:5051/swagger](http://localhost:5051/swagger)
