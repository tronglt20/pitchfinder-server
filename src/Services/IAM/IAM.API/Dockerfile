#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/IAM/IAM.API/IAM.API.csproj", "Services/IAM/IAM.API/"]
COPY ["Services/Shared.Service/Shared.Service.csproj", "Services/Shared.Service/"]
COPY ["Utilities/PitchFinder.S3/PitchFinder.S3.csproj", "Utilities/PitchFinder.S3/"]
COPY ["Services/Shared.Domain/Shared.Domain.csproj", "Services/Shared.Domain/"]
COPY ["Services/Shared.Infrastructure/Shared.Infrastructure.csproj", "Services/Shared.Infrastructure/"]
COPY ["Services/IAM/IAM.Infrastructure/IAM.Infrastructure.csproj", "Services/IAM/IAM.Infrastructure/"]
COPY ["Services/IAM/IAM.Domain/IAM.Domain.csproj", "Services/IAM/IAM.Domain/"]
RUN dotnet restore "Services/IAM/IAM.API/IAM.API.csproj"
COPY . .
WORKDIR "/src/Services/IAM/IAM.API"
RUN dotnet build "IAM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IAM.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IAM.API.dll"]