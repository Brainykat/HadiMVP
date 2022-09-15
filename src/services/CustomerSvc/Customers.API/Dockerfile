#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/CustomerSvc/Customers.API/Customers.API.csproj", "src/services/CustomerSvc/Customers.API/"]
COPY ["src/services/CustomerSvc/Customers.Data/Customers.Data.csproj", "src/services/CustomerSvc/Customers.Data/"]
COPY ["src/services/CustomerSvc/Customers.Domain/Customers.Domain.csproj", "src/services/CustomerSvc/Customers.Domain/"]
COPY ["src/shared/Common.Base.Shared/Common.Base.Shared.csproj", "src/shared/Common.Base.Shared/"]
COPY ["src/shared/EventBus/RabbitMQ/RabbitMQ.csproj", "src/shared/EventBus/RabbitMQ/"]
COPY ["src/services/CustomerSvc/Customers.Services/Customers.Services.csproj", "src/services/CustomerSvc/Customers.Services/"]
COPY ["CustomerBusinessDtos/CustomerBusinessDtos.csproj", "CustomerBusinessDtos/"]
RUN dotnet restore "src/services/CustomerSvc/Customers.API/Customers.API.csproj"
COPY . .
WORKDIR "/src/src/services/CustomerSvc/Customers.API"
RUN dotnet build "Customers.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customers.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customers.API.dll"]