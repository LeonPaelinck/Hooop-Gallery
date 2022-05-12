#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
USER root
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Project3H04/Server/Project3H04.Server.csproj", "Project3H04/Server/"]
COPY ["Project3H04/Client/Project3H04.Client.csproj", "Project3H04/Client/"]
COPY ["Project3H04/Shared/Project3H04.Shared.csproj", "Project3H04/Shared/"]
COPY ["/Domain/Domain.csproj", "Domain/Domain/"]
RUN dotnet restore "Project3H04/Server/Project3H04.Server.csproj"
COPY . .
WORKDIR "/src/Project3H04/Server"
RUN dotnet build "Project3H04.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Project3H04.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project3H04.Server.dll"]
