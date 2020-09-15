FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Ddb.Api/Ddb.Api.csproj", "Ddb.Api/"]
RUN dotnet restore "Ddb.Api/Ddb.Api.csproj"
COPY . .
WORKDIR "/src/Ddb.Api"
RUN dotnet build "Ddb.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ddb.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ddb.Api.dll"]
