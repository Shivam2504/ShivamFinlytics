# STAGE 1: Build the application using the .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy the .csproj files for all projects to restore dependencies
# This allows Docker to cache the restore step and build faster next time
COPY ["ShivamFinlytyics.API/ShivamFinlytyics.API.csproj", "ShivamFinlytyics.API/"]
COPY ["ShivamFinlytics.Application/ShivamFinlytics.Application.csproj", "ShivamFinlytics.Application/"]
COPY ["ShivamFinlytics.Domain/ShivamFinlytics.Domain.csproj", "ShivamFinlytics.Domain/"]
COPY ["ShivamFinlytics.Infrastructure/ShivamFinlytics.Infrastructure.csproj", "ShivamFinlytics.Infrastructure/"]

# 2. Restore all NuGet packages
RUN dotnet restore "ShivamFinlytyics.API/ShivamFinlytyics.API.csproj"

# 3. Copy the entire source code into the container
COPY . .

# 4. Build and Publish the API project in Release mode
WORKDIR "/src/ShivamFinlytyics.API"
RUN dotnet publish "ShivamFinlytyics.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# STAGE 2: Create the final lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# 5. Copy the published files from the build stage
COPY --from=build /app/publish .

# 6. Set Environment Variables for Cloud Run
# Google Cloud Run expects the app to listen on the port provided in the PORT variable
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# 7. Start the application
ENTRYPOINT ["dotnet", "ShivamFinlytyics.API.dll"]