FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CommandLine/CommandLine.csproj", "CommandLine/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Discord Bot/Discord Bot.csproj", "Discord Bot/"]
RUN dotnet restore "CommandLine/CommandLine.csproj"
COPY . .
WORKDIR "/src/CommandLine"
RUN dotnet build "./CommandLine.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CommandLine.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommandLine.dll"]
