# Sử dụng .NET SDK 9.0 để build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Sao chép tệp dự án và khôi phục dependencies
COPY *.csproj ./
RUN dotnet restore

# Sao chép toàn bộ source code và build ứng dụng
COPY . ./
RUN dotnet publish -c Release -o /publish

# Sử dụng .NET runtime 9.0 để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "WebsiteBanHang.dll"]