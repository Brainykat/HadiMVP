#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/OrderSvc/Ordering.API/Ordering.API.csproj", "src/services/OrderSvc/Ordering.API/"]
COPY ["src/services/OrderSvc/Ordering.Data/Ordering.Data.csproj", "src/services/OrderSvc/Ordering.Data/"]
COPY ["src/services/OrderSvc/Ordering.Domain/Ordering.Domain.csproj", "src/services/OrderSvc/Ordering.Domain/"]
COPY ["src/shared/Common.Base.Shared/Common.Base.Shared.csproj", "src/shared/Common.Base.Shared/"]
COPY ["src/services/OrderSvc/Ordering.Data.Mongo/Ordering.Data.Mongo.csproj", "src/services/OrderSvc/Ordering.Data.Mongo/"]
COPY ["src/shared/EventBus/RabbitMQ/RabbitMQ.csproj", "src/shared/EventBus/RabbitMQ/"]
COPY ["src/services/OrderSvc/Ordering.Services/Ordering.Services.csproj", "src/services/OrderSvc/Ordering.Services/"]
COPY ["src/shared/Dtos/OrderingDtos/OrderingDtos.csproj", "src/shared/Dtos/OrderingDtos/"]
RUN dotnet restore "src/services/OrderSvc/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/services/OrderSvc/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]