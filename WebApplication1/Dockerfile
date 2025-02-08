FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.sln .
COPY ./WebApplication1.csproj ./
RUN dotnet restore "WebApplication1.csproj"

# Copy the rest of the files and build
COPY . .
RUN dotnet publish ./WebApplication1.csproj -c Release -o /app/

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "WebApplication1.dll"] 