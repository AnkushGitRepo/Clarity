FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Clarity.Api.csproj", "./"]
RUN dotnet restore "./Clarity.Api.csproj"
COPY . .
RUN dotnet publish "Clarity.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Clarity.Api.dll"]
