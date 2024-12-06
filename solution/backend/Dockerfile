FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/MoviesChallenge.Api/MoviesChallenge.Api.csproj", "backend/MoviesChallenge.Api/"]
COPY ["backend/MoviesChallenge.Application/MoviesChallenge.Application.csproj", "backend/MoviesChallenge.Application/"]
COPY ["backend/MoviesChallenge.Domain/MoviesChallenge.Domain.csproj", "backend/MoviesChallenge.Domain/"]
COPY ["backend/MoviesChallenge.Infra/MoviesChallenge.Infra.csproj", "backend/MoviesChallenge.Infra/"]
RUN dotnet restore "./backend/MoviesChallenge.Api/MoviesChallenge.Api.csproj"
COPY . .
WORKDIR "/src/backend/MoviesChallenge.Api"
RUN dotnet build "./MoviesChallenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MoviesChallenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoviesChallenge.Api.dll"]