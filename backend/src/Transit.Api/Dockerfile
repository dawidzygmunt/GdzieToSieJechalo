FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./
COPY src/Transit.Api/Transit.Api.csproj           src/Transit.Api/
COPY src/Transit.Application/Transit.Application.csproj src/Transit.Application/
COPY src/Transit.Domain/Transit.Domain.csproj     src/Transit.Domain/
COPY src/Transit.Infrastructure/Transit.Infrastructure.csproj src/Transit.Infrastructure/

RUN dotnet restore src/Transit.Api/Transit.Api.csproj

COPY src/ src/

RUN dotnet publish src/Transit.Api/Transit.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "Transit.Api.dll"]
