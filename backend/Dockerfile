# 1) Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2) Build SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the csproj and restore (for layer caching)
COPY StudyTracker.csproj ./
RUN dotnet restore "StudyTracker.csproj"

# Copy the rest of the sources
COPY . ./

WORKDIR /src
RUN dotnet build "StudyTracker.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# 3) Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudyTracker.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# 4) Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyTracker.dll"]
