#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Order/Order.Grpc/Order.Grpc.csproj", "Services/Order/Order.Grpc/"]
COPY ["Services/Order/Order.Domain/Order.Domain.csproj", "Services/Order/Order.Domain/"]
COPY ["Services/Shared.Domain/Shared.Domain.csproj", "Services/Shared.Domain/"]
COPY ["Services/Order/Order.Infrastructure/Order.Infrastructure.csproj", "Services/Order/Order.Infrastructure/"]
COPY ["Services/Shared.Infrastructure/Shared.Infrastructure.csproj", "Services/Shared.Infrastructure/"]
COPY ["Services/Shared.API/Shared.API.csproj", "Services/Shared.API/"]
COPY ["Utilities/PitchFinder.S3/PitchFinder.S3.csproj", "Utilities/PitchFinder.S3/"]
RUN dotnet restore "Services/Order/Order.Grpc/Order.Grpc.csproj"
COPY . .
WORKDIR "/src/Services/Order/Order.Grpc"
RUN dotnet build "Order.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Grpc.dll"]
