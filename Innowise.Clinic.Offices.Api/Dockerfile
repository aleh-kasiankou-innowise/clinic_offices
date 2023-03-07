FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Innowise.Clinic.Offices.Api/NuGet.Config", "./"]
COPY ["Innowise.Clinic.Offices.Api/Innowise.Clinic.Offices.Api.csproj", "Innowise.Clinic.Offices.Api/"]
RUN dotnet restore "Innowise.Clinic.Offices.Api/Innowise.Clinic.Offices.Api.csproj"
COPY . .
WORKDIR "/src/Innowise.Clinic.Offices.Api"
RUN dotnet build "Innowise.Clinic.Offices.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innowise.Clinic.Offices.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innowise.Clinic.Offices.Api.dll"]
