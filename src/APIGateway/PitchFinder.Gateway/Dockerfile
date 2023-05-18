#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["APIGateway/PitchFinder.Gateway/PitchFinder.Gateway.csproj", "APIGateway/PitchFinder.Gateway/"]
RUN dotnet restore "APIGateway/PitchFinder.Gateway/PitchFinder.Gateway.csproj"
COPY . .
WORKDIR "/src/APIGateway/PitchFinder.Gateway"
RUN dotnet build "PitchFinder.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PitchFinder.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PitchFinder.Gateway.dll"]
