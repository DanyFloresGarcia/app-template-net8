# Crear un Proyecto .NET 8 con Clean Architecture
## Configurar un Ambiente Docker para .NET 8

Antes de crear la solución, puedes configurar un ambiente Docker local para trabajar con .NET 8 sin necesidad de instalarlo directamente en tu máquina. A continuación, se detallan los pasos:

### Pasos

1. **Instalar Docker**:
  Asegúrate de tener Docker instalado en tu máquina. Puedes descargarlo desde [Docker Desktop](https://www.docker.com/products/docker-desktop/).

2. **Crear un archivo `Dockerfile`**:
  En el directorio raíz de tu proyecto, crea un archivo llamado `Dockerfile` con el siguiente contenido:
  ```dockerfile
  FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
  WORKDIR /app
  ```

3. **Crear un contenedor interactivo**:
  Ejecuta el siguiente comando para iniciar un contenedor interactivo con la imagen de .NET 8:
  ```bash
  docker run -it --rm -v ${PWD}:/app -w /app mcr.microsoft.com/dotnet/sdk:8.0 bash
  ```

  Este comando monta tu directorio actual en el contenedor y te permite ejecutar comandos de .NET dentro del contenedor.

4. **Verificar la instalación de .NET**:
  Dentro del contenedor, verifica que .NET 8 está instalado ejecutando:
  ```bash
  dotnet --version
  ```

5. **Continuar con la creación de la solución**:
  Ahora puedes seguir los pasos descritos anteriormente para crear la solución y los proyectos dentro del contenedor Docker.

Con este enfoque, puedes trabajar con .NET 8 sin instalarlo directamente en tu máquina, manteniendo tu entorno limpio.
A continuación, se explica cómo crear un proyecto en .NET 8 utilizando el enfoque de Clean Architecture mediante comandos:
## Pasos

1. **Crear la solución principal**:
  ```bash
  dotnet new sln -n AppTemplateNet8
  ```

2. **Crear las carpetas para las capas**:
  En el directorio raíz, crea una carpeta llamada `src` y dentro de ella, crea cuatro subcarpetas para las capas de la arquitectura limpia:
  ```bash
  mkdir -p src/Application src/Domain src/Infrastructure src/API
  ```

3. **Crear los proyectos dentro de las carpetas correspondientes**:
  - **Capa de Aplicación**:
    ```bash
    dotnet new classlib -n Application -o src/Application
    ```
  - **Capa de Dominio**:
    ```bash
    dotnet new classlib -n Domain -o src/Domain
    ```
  - **Capa de Infraestructura**:
    ```bash
    dotnet new classlib -n Infrastructure -o src/Infrastructure
    ```
  - **Capa de Presentación (API)**:
    ```bash
    dotnet new webapi -n API -o src/API
    ```

4. **Agregar los proyectos a la solución**:
  ```bash
  dotnet sln add src/Application/Application.csproj
  dotnet sln add src/Domain/Domain.csproj
  dotnet sln add src/Infrastructure/Infrastructure.csproj
  dotnet sln add src/API/API.csproj
  ```

5. **Establecer las referencias entre proyectos**:
  ```bash
  dotnet add src/API/API.csproj reference src/Application/Application.csproj
  dotnet add src/API/API.csproj reference src/Infrastructure/Infrastructure.csproj
  dotnet add src/Infrastructure/Infrastructure.csproj reference src/Application/Application.csproj
  dotnet add src/Infrastructure/Infrastructure.csproj reference src/Domain/Domain.csproj
  dotnet add src/Application/Application.csproj reference src/Domain/Domain.csproj
  ```

6. **Ejecutar el proyecto**:
  Navega al directorio del proyecto API y ejecuta:
  ```bash
  dotnet run 
  o
  dotnet run --project src/API/API.csproj
  ```
7. **Instalar ef global**:
  Navega al directorio del proyecto API y ejecuta:
  ```
    dotnet tool install --global dotnet-ef
    export PATH="$PATH:/root/.dotnet/tools"
  ```

  o tambien si es que es necesario
  ```
  dotnet tool restore
  ```
8. **Crear archivo de migracion**:
  Navega al directorio del proyecto API y ejecuta:
  ```
    dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/API
  ```

9. **Ejecutar la migracion**:
  Navega al directorio del proyecto API y ejecuta:
  ```
    dotnet ef database update --project src/Infrastructure --startup-project src/API
  ```  


Con esta estructura, los proyectos estarán organizados dentro de la carpeta `src`, manteniendo un diseño limpio y escalable.

¡Listo! Ahora tienes una solución básica con Clean Architecture en .NET 8.

