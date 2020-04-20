#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
LABEL Roleen Moreland

WORKDIR /CoviIDApiCore
COPY /*.csproj ./

RUN dotnet restore
COPY . .

RUN dotnet publish --output /api/ --configuration Release
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /api
COPY --from=builder /api .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "CoviIDApiCore.dll"]