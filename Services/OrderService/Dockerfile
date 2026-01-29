FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["OrderService/OrderService.API/OrderService.API.csproj", "OrderService.API/"]
COPY ["OrderService/OrderService.Application/OrderService.Application.csproj", "OrderService.Application/"]
COPY ["OrderService/OrderService.Domain/OrderService.Domain.csproj", "OrderService.Domain/"]
COPY ["OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService.Infrastructure/"]
COPY ["Common/Web/Web.csproj", "Common.Web/"]
COPY ["Common/Common.Messaging/Common.Messaging.csproj", "Common.Common.Messaging/"]
RUN dotnet restore "OrderService.API/OrderService.API.csproj"

COPY . .
RUN dotnet build "OrderService/OrderService.API/OrderService.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "OrderService/OrderService.API/OrderService.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
