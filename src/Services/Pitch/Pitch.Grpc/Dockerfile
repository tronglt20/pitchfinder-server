#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Pitch/Pitch.Grpc/Pitch.Grpc.csproj", "Services/Pitch/Pitch.Grpc/"]
COPY ["Services/Pitch/Pitch.Infrastructure/Pitch.Infrastructure.csproj", "Services/Pitch/Pitch.Infrastructure/"]
COPY ["Services/Shared.Infrastructure/Shared.Infrastructure.csproj", "Services/Shared.Infrastructure/"]
COPY ["Services/Shared.Domain/Shared.Domain.csproj", "Services/Shared.Domain/"]
COPY ["Services/Pitch/Pitch.Domain/Pitch.Domain.csproj", "Services/Pitch/Pitch.Domain/"]
COPY ["Services/Shared.API/Shared.API.csproj", "Services/Shared.API/"]
COPY ["Utilities/PitchFinder.S3/PitchFinder.S3.csproj", "Utilities/PitchFinder.S3/"]
RUN dotnet restore "Services/Pitch/Pitch.Grpc/Pitch.Grpc.csproj"
COPY . .
WORKDIR "/src/Services/Pitch/Pitch.Grpc"
RUN dotnet build "Pitch.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pitch.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pitch.Grpc.dll"]
