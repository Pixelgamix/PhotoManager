FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY PhotoManager.Contracts/PhotoManager.Contracts.csproj ./PhotoManager.Contracts/
COPY PhotoManager.DataAccess/PhotoManager.DataAccess.csproj ./PhotoManager.DataAccess/
COPY PhotoManager.BusinessService/PhotoManager.BusinessService.csproj ./PhotoManager.BusinessService/
COPY PhotoManager.WebApi/PhotoManager.WebApi.csproj ./PhotoManager.WebApi/

WORKDIR /app/PhotoManager.WebApi
RUN dotnet restore 

# Copy everything else and build
WORKDIR /app
COPY PhotoManager.Contracts/. ./PhotoManager.Contracts/
COPY PhotoManager.DataAccess/. ./PhotoManager.DataAccess/
COPY PhotoManager.BusinessService/. ./PhotoManager.BusinessService/
COPY PhotoManager.WebApi/. ./PhotoManager.WebApi/

WORKDIR /app/PhotoManager.WebApi

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/PhotoManager.WebApi/out .
ENTRYPOINT ["dotnet", "PhotoManager.WebApi.dll"]
