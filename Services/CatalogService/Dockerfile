# Base stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
# USER app  <-- remove or create user if needed

# Build stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src

COPY ["Services/CatalogService/CatalogService.API/CatalogService.API.csproj", "CatalogService.API/"]
COPY ["Services/CatalogService/CatalogService.Application/CatalogService.Application.csproj", "CatalogService.Application/"]
COPY ["Services/CatalogService/CatalogService.Domain/CatalogService.Domain.csproj", "CatalogService.Domain/"]
COPY ["Services/CatalogService/CatalogService.Infrastructure/CatalogService.Infrastructure.csproj", "CatalogService.Infrastructure/"]
COPY ["Services/Common/Web/Web.csproj", "Common.Web/"]

RUN dotnet restore "CatalogService.API/CatalogService.API.csproj"
COPY . .
RUN dotnet build "Services/CatalogService/CatalogService.API/CatalogService.API.csproj" -c $configuration -o /app/build

# Publish stage
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Services/CatalogService/CatalogService.API/CatalogService.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.API.dll"]
