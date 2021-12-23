#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CompaniesService/CompaniesService.csproj", "CompaniesService/"]
COPY ["TestNHibernate/TestNHibernate.csproj", "TestNHibernate/"]
COPY ["ConstData/ConstData.csproj", "ConstData/"]
COPY ["ServicesInterfaces/ServicesInterfaces.csproj", "ServicesInterfaces/"]
COPY ["TestBaseDto/TestBaseDto.csproj", "TestBaseDto/"]
COPY ["TestBase/TestBase.csproj", "TestBase/"]
RUN dotnet restore "CompaniesService/CompaniesService.csproj"
COPY . .
WORKDIR "/src/CompaniesService"
RUN dotnet build "CompaniesService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompaniesService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompaniesService.dll"]
