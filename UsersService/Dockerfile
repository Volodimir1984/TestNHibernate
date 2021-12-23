#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["UsersService/UsersService.csproj", "UsersService/"]
COPY ["ConstData/ConstData.csproj", "ConstData/"]
COPY ["ServicesInterfaces/ServicesInterfaces.csproj", "ServicesInterfaces/"]
COPY ["TestBaseDto/TestBaseDto.csproj", "TestBaseDto/"]
COPY ["TestBase/TestBase.csproj", "TestBase/"]

RUN dotnet restore "UsersService/UsersService.csproj"
COPY . .
WORKDIR "/src/UsersService"
RUN dotnet build "UsersService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsersService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersService.dll"]