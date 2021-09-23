#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/OrderService/OrderService.csproj", "Services/OrderService/"]
COPY ["Common/Repository/Repository.csproj", "Common/Repository/"]
COPY ["Common/Entities/Entities.csproj", "Common/Entities/"]
COPY ["Common/CommonLib/CommonLib.csproj", "Common/CommonLib/"]
RUN dotnet restore "Services/OrderService/OrderService.csproj"
COPY . .
WORKDIR "/src/Services/OrderService"
RUN dotnet build "OrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]
