# Bước 1: Dùng image ASP.NET Core runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Bước 2: Dùng image .NET SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebsiteBanHangT6.csproj", "./"]
RUN dotnet restore "./WebsiteBanHangT6.csproj"
COPY . .
RUN dotnet publish "./WebsiteBanHangT6.csproj" -c Release -o /app/publish

# Bước 3: Chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebsiteBanHangT6.dll"]
