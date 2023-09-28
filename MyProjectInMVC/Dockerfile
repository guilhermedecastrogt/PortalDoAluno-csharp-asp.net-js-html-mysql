#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProjectInMVC/MyProjectInMVC.csproj", "MyProjectInMVC/"]
RUN dotnet restore "MyProjectInMVC/MyProjectInMVC.csproj"
COPY . .
WORKDIR "/src/MyProjectInMVC"
RUN dotnet build "MyProjectInMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProjectInMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

ARG ConnectionStringName
ENV ConnectionStringName=$ConnectionStringName

ARG FtpConnectionUsername
ENV FtpConnectionUsername=$FtpConnectionUsername
ARG FtpConnectionPassword
ENV FtpConnectionPassword=$FtpConnectionPassword
ARG FtpConnectionServerUrl
ENV FtpConnectionServerUrl=$FtpConnectionServerUrl

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProjectInMVC.dll"]