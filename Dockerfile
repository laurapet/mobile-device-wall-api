FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 4000

ENV ASPNETCORE_URLS=http://+:4000

FROM postgres:latest
EXPOSE 5432

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["device-wall-backend.csproj", "./"]
RUN dotnet restore "device-wall-backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "device-wall-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "device-wall-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "device-wall-backend.dll"]
