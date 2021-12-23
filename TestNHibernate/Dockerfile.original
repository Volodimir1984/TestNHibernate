#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TestNHibernate/TestNHibernate.csproj", "TestNHibernate/"]
COPY ["ConstData/ConstData.csproj", "ConstData/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["ServicesInterfaces/ServicesInterfaces.csproj", "ServicesInterfaces/"]
COPY ["TestBaseDto/TestBaseDto.csproj", "TestBaseDto/"]
COPY ["TestBase/TestBase.csproj", "TestBase/"]
RUN dotnet restore "TestNHibernate/TestNHibernate.csproj"
COPY . .
WORKDIR "/src/TestNHibernate"
RUN dotnet build "TestNHibernate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestNHibernate.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestNHibernate.dll"]