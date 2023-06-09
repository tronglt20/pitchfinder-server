#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Payment/Payment.Momo/Payment.Momo.csproj", "Services/Payment/Payment.Momo/"]
COPY ["ServiceBus/PitchFinder.RambitMQ/PitchFinder.RambitMQ.csproj", "ServiceBus/PitchFinder.RambitMQ/"]
COPY ["Services/Shared.API/Shared.API.csproj", "Services/Shared.API/"]
COPY ["Utilities/PitchFinder.S3/PitchFinder.S3.csproj", "Utilities/PitchFinder.S3/"]
COPY ["Services/Shared.Infrastructure/Shared.Infrastructure.csproj", "Services/Shared.Infrastructure/"]
COPY ["Services/Shared.Domain/Shared.Domain.csproj", "Services/Shared.Domain/"]
RUN dotnet restore "Services/Payment/Payment.Momo/Payment.Momo.csproj"
COPY . .
WORKDIR "/src/Services/Payment/Payment.Momo"
RUN dotnet build "Payment.Momo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.Momo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Momo.dll"]
