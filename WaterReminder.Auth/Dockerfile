# See https://aka.ms/customizecontainer to learn how to customize your container.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WaterReminder.Auth.csproj", "./"]
RUN dotnet restore "./WaterReminder.Auth.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./WaterReminder.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WaterReminder.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WaterReminder.Auth.dll"]
