FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#RUN mkdir -p /app/appData && \
    #chown -R $APP_UID:$APP_UID /app/appData && \
    #chmod -R 755 /app/appData

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EVS.App/EVS.App.csproj", "EVS.App/"]
RUN dotnet restore "EVS.App/EVS.App.csproj"
COPY . .
WORKDIR "/src/EVS.App"
RUN dotnet build "EVS.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EVS.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EVS.App.dll"]
