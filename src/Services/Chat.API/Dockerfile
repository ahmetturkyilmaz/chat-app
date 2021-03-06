#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Chat.API/Chat.API.csproj", "Services/Chat.API/"]
COPY ["BuildingBlocks/ClassLibrary1/EventBus.Messages.csproj", "BuildingBlocks/ClassLibrary1/"]
RUN dotnet restore "Services/Chat.API/Chat.API.csproj"
COPY . .
WORKDIR "/src/Services/Chat.API"
RUN dotnet build "Chat.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.API.dll"]