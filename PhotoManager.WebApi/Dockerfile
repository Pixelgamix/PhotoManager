#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["PhotoManager.WebApi/PhotoManager.WebApi.csproj", "PhotoManager.WebApi/"]
COPY ["PhotoManager.DataAccess/PhotoManager.DataAccess.csproj", "PhotoManager.DataAccess/"]
COPY ["PhotoManager.Contracts/PhotoManager.Contracts.csproj", "PhotoManager.Contracts/"]
COPY ["PhotoManager.BusinessService/PhotoManager.BusinessService.csproj", "PhotoManager.BusinessService/"]
RUN dotnet restore "PhotoManager.WebApi/PhotoManager.WebApi.csproj"
COPY . .
WORKDIR "/src/PhotoManager.WebApi"
RUN dotnet build "PhotoManager.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhotoManager.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhotoManager.WebApi.dll"]
