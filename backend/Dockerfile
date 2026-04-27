FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Nexus.Api/Nexus.Api.csproj", "src/Nexus.Api/"]
RUN dotnet restore "src/Nexus.Api/Nexus.Api.csproj"
COPY . .
WORKDIR "/src/src/Nexus.Api"
RUN dotnet build "Nexus.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexus.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexus.Api.dll"]
