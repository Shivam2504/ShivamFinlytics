FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ShivamFinlytyics.API/ShivamFinlytyics.API.csproj", "ShivamFinlytyics.API/"]
COPY ["ShivamFinlytics.Application/ShivamFinlytics.Application.csproj", "ShivamFinlytics.Application/"]
COPY ["ShivamFinlytics.Domain/ShivamFinlytics.Domain.csproj", "ShivamFinlytics.Domain/"]
COPY ["ShivamFinlytics.Infrastructure/ShivamFinlytics.Infrastructure.csproj", "ShivamFinlytics.Infrastructure/"]

RUN dotnet restore "ShivamFinlytyics.API/ShivamFinlytyics.API.csproj"

COPY . .

WORKDIR "/src/ShivamFinlytyics.API"
RUN dotnet publish "ShivamFinlytyics.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app


COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ShivamFinlytyics.API.dll"]