# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# Copiar los archivos .csproj y restaurar las dependencias
COPY *.sln .
COPY CommunWork/CommunWork.csproj CommunWork/
COPY SubscriptoAplicaction/SuscriptorAplicaction.csproj SubscriptoAplicaction/

RUN dotnet restore

# Copiar todo el código fuente y construir el proyecto
COPY . .
WORKDIR /source/SubscriptoAplicaction
RUN dotnet publish -c Release -o /app

# Etapa de producción
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Exponer los puertos HTTP y HTTPS utilizados
EXPOSE 5054
EXPOSE 7016

# Configurar la aplicación para que escuche en los puertos correctos
ENV ASPNETCORE_URLS="http://+:5054;https://+:7016"

# Comando de inicio
ENTRYPOINT ["dotnet", "SuscriptorAplicaction.dll"]
