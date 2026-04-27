# STAGE 1: Frontend Build
FROM node:20-alpine AS frontend-build
WORKDIR /app
COPY frontend/package*.json ./
RUN npm install
COPY frontend/ ./
RUN npm run build

# STAGE 2: Backend Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build
WORKDIR /src
COPY ["backend/src/Nexus.Api/Nexus.Api.csproj", "src/Nexus.Api/"]
COPY ["backend/src/Nexus.Bff/Nexus.Bff.csproj", "src/Nexus.Bff/"]
RUN dotnet restore "src/Nexus.Bff/Nexus.Bff.csproj"
COPY backend/src/ ./src/
WORKDIR "/src/src/Nexus.Bff"
RUN dotnet publish "Nexus.Bff.csproj" -c Release -o /app/publish

# STAGE 3: Final Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=backend-build /app/publish .
# Copy frontend dist to wwwroot of BFF
COPY --from=frontend-build /app/dist ./wwwroot
EXPOSE 8080
ENTRYPOINT ["dotnet", "Nexus.Bff.dll"]
