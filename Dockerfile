# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first (better layer caching)

COPY BIzKidzScholarships.API/BizKidzScholarships.API.csproj BIzKidzScholarships.API/
COPY BizKidzScholarships.Data/BizKidzScholarships.Data.csproj BizKidzScholarships.Data/

# Restore only API project (this pulls Data automatically)
RUN dotnet restore BIzKidzScholarships.API/BizKidzScholarships.API.csproj

# Copy everything except what's ignored by .dockerignore
COPY . .

# Publish API
RUN dotnet publish BIzKidzScholarships.API/BizKidzScholarships.API.csproj -c Release -o /app/out

# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BizKidzScholarships.API.dll"]