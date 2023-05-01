FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["vbr-state-machine-console/vbr-state-machine-console.csproj", "vbr-state-machine-console/"]
RUN dotnet restore "vbr-state-machine-console/vbr-state-machine-console.csproj"
COPY . .
WORKDIR "/src/vbr-state-machine-console"
RUN dotnet build "vbr-state-machine-console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vbr-state-machine-console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vbr-state-machine-console.dll"]