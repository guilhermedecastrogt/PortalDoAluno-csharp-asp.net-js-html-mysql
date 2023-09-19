# Veja https://aka.ms/customizecontainer para aprender como personalizar seu contêiner de depuração
# e como o Visual Studio utiliza este Dockerfile para construir suas imagens para depuração mais rápida.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProjectInMVC/MyProjectInMVC.csproj", "MyProjectInMVC/"]
RUN dotnet restore "MyProjectInMVC/MyProjectInMVC.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "MyProjectInMVC/MyProjectInMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProjectInMVC/MyProjectInMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Defina a variável de ambiente para a connection string
ENV ConnectionStringName="server=212.192.29.71;user=seusitep_portaldoalunouser;database=seusitep_PortalDoAluno;port=3306;password=123321Qwa.123321"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProjectInMVC.dll"]