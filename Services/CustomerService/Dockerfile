#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/CustomerService/CustomerService.csproj", "Services/CustomerService/"]
COPY ["Common/Repository/Repository.csproj", "Common/Repository/"]
COPY ["Common/Entities/Entities.csproj", "Common/Entities/"]
COPY ["Common/CommonLib/CommonLib.csproj", "Common/CommonLib/"]
RUN dotnet restore "Services/CustomerService/CustomerService.csproj"
COPY . .
WORKDIR "/src/Services/CustomerService"
RUN dotnet build "CustomerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.dll"]
