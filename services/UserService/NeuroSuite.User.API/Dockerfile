# 1. Base image: .NET 9.0 Runtime (preview)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8080

# 2. Build image: .NET 9.0 SDK (preview)
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src

# 3. T�m ��z�m klas�r�n� al
COPY . .

# 4. Restore ve build i�lemleri
RUN dotnet restore "services/UserService/NeuroSuite.User.API/NeuroSuite.User.API.csproj"
RUN dotnet build "services/UserService/NeuroSuite.User.API/NeuroSuite.User.API.csproj" -c Release -o /app/build

# 5. Yay�nlama (publish)
FROM build AS publish
RUN dotnet publish "services/UserService/NeuroSuite.User.API/NeuroSuite.User.API.csproj" -c Release -o /app/publish

# 6. Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NeuroSuite.User.API.dll"]
