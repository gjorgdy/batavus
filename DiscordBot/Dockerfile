FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Discord Bot/Discord Bot.csproj", "Discord Bot/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["CoreModules/CoreModules.csproj", "CoreModules/"]
RUN dotnet restore "Discord Bot/Discord Bot.csproj"
COPY . .
WORKDIR "/src/Discord Bot"
RUN dotnet build "./Discord Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Discord Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discord Bot.dll"]
