FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/CartService/CartService.API/CartService.API.csproj", "CartService.API/"]
COPY ["Services/CartService/CartService.Domain/CartService.Domain.csproj", "CartService.Domain/"]
COPY ["Services/CartService/CartService.Application/CartService.Application.csproj", "CartService.Application/"]
COPY ["Services/CartService/CartService.Infrastructure/CartService.Infrastructure.csproj", "CartService.Infrastructure/"]
COPY ["Services/Common/Web/Web.csproj", "Common.Web/"]

RUN dotnet restore "CartService.API/CartService.API.csproj"
COPY . .
RUN dotnet build "Services/CartService/CartService.API/CartService.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Services/CartService/CartService.API/CartService.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartService.API.dll"]
