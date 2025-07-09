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


Con esta estructura, los proyectos estarán organizados dentro de la carpeta `src`, manteniendo un diseño limpio y escalable.

¡Listo! Ahora tienes una solución básica con Clean Architecture en .NET 8.


## Comandos EF
1. **Instalar ef global**:
  Navega al directorio del proyecto API y ejecuta:
  ```
    dotnet tool install --global dotnet-ef
    export PATH="$PATH:/root/.dotnet/tools"
  ```

  o tambien si es que es necesario
  ```
  dotnet tool restore
  ```
## Migracion EF
  ### Migration Database
 ```
  dotnet ef migrations add InitialCreatePostgreSQL --context ApplicationDbContextPostgreSql --output-dir Persistence/Migrations/PostgreSQL --project src/Infrastructure --startup-project src/API
 ```

 ### Actualizar Database
  ```
    dotnet ef database update --context ApplicationDbContextPostgreSql --project src/Infrastructure --startup-project src/API
  ```

## Commandos Docker

1. **Entrar en el sh del contenedor docker**
  Navega y ejecuta el siguiente comando:
  ```
    docker exec -it <CONTAINER_NAME> sh
  ```  

2. **Instalar telnet y ping en docker**
  Navega y ejecuta el siguiente comando:
  ```
    docker run -it --rm --network app-template-net8-app-1 nicolaka/netshoot
  ```  
3. **Listar conexiones de contenedores**
   Navega y ejecuta el siguiente comando:
  ```
    docker network ls
  ```  

  ## Crear certificado en Dev

1. **Crear**
  ```  
    dotnet dev-certs https
  ```  

# Lambda

## 1. Create carpeta lambda
```
  mkdir -p src/Lambdas/CreateInvitadoLambda
  cd src/Lambdas/CreateInvitadoLambda
```
## 2. Crear proyecto lambda
```
  dotnet new console -n CreateInvitadoLambda
```
## 3. Paquetes necesarios 

cd CreateInvitadoLambda

```
  dotnet add package Amazon.Lambda.Core
  dotnet add package Amazon.Lambda.Serialization.SystemTextJson
  dotnet add package Microsoft.Extensions.DependencyInjection
```
## 4. Agregar referencias
```
  dotnet add reference ../../../Application/Application.csproj
  dotnet add reference ../../../Domain/Domain.csproj
  dotnet add reference ../../../Infrastructure/Infrastructure.csproj
```

## 5. Agregar el archivo aws-lambda-tools-defaults.json
```
  touch aws-lambda-tools-defaults.json
```
Contenido:
{
  "Information": [ "Config for Lambda Test Tool" ],
  "profile": "default",
  "region": "us-east-1",
  "configuration": "Release",
  "function-runtime": "dotnet8",
  "function-handler": "CreateInvitadoLambda::CreateInvitadoLambda.Function::FunctionHandler",
  "framework": "net8.0",
  "function-name": "CreateInvitadoLambda",
  "function-memory-size": 256,
  "function-timeout": 30,
  "function-role": "arn:aws:iam::123456789012:role/lambda_exec_role"
}

## 6. Eliminar Program.cs

## 7. Crear archivo Function.cs

## 8. Agregar al proyecto solucion desde la raiz del proyecto
```
  dotnet sln add src/Lambdas/CreateInvitadoLambda/CreateInvitadoLambda.csproj
```
## 9. Instalar globalmente el amazon lambda Tools para probar, ejecutar en un cmd global
```
  dotnet tool install -g Amazon.Lambda.Tools
  dotnet tool install -g Amazon.Lambda.TestTool-8.0 --version 0.16.2
```

## 10. Cambiar en el .csproj de
<OutputType>Exe</OutputType>  a <OutputType>Library</OutputType> 

## 11. Probar Test Lambda
cd src/Lambdas/CreateInvitadoLambda
```
  dotnet build
  dotnet-lambda-test-tool-8.0
```

## 12. Agregar referencia en la lambda
dotnet add package Amazon.Lambda.RuntimeSupport

## 13. Payload

{
  "name": "dany",
  "lastame": "flores",
  "email": "dflores@acity.com.pe",
  "phone": "994157568",
  "userCreated": "dflores",
  "applicationName": "localhost"
},
{
  "name": "roberto",
  "lastame": "gomez",
  "email": "ggomez@acity.com.pe",
  "phone": "998565258",
  "userCreated": "dflores",
  "applicationName": "localhost"
}


## Payload Pruebas Lambda

{
  "body": "{ \"name\": \"roberto\", \"lastame\": \"gomez\", \"email\": \"ggomez@acity.com.pe\", \"phone\": \"998565258\", \"userCreated\": \"dflores\", \"applicationName\": \"localhost\" }",
  "httpMethod": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "isBase64Encoded": false,
  "path": "/",
  "queryStringParameters": null
}

{
  "body": "{ \"name\": \"cris\", \"lastame\": \"martinez\", \"email\": \"cmartinez@acity.com.pe\", \"phone\": \"995745258\", \"userCreated\": \"dflores\", \"applicationName\": \"localhost\" }",
  "httpMethod": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "isBase64Encoded": false,
  "path": "/",
  "queryStringParameters": null
}

{
  "body": "{ \"name\": \"sofia\", \"lastame\": \"nieto\", \"email\": \"snieto@acity.com.pe\", \"phone\": \"995745855\", \"userCreated\": \"dflores\", \"applicationName\": \"localhost\" }",
  "httpMethod": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "isBase64Encoded": false,
  "path": "/",
  "queryStringParameters": null
}

{
  "body": "{ \"name\": \"victor\", \"lastame\": \"poma\", \"email\": \"vpoma@acity.com.pe\", \"phone\": \"975742856\", \"userCreated\": \"dflores\", \"applicationName\": \"localhost\" }",
  "httpMethod": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "isBase64Encoded": false,
  "path": "/",
  "queryStringParameters": null
}

{
  "body": "{ \"name\": \"raul\", \"lastame\": \"cucurella\", \"email\": \"rcucurella@acity.com.pe\", \"phone\": \"975742256\", \"userCreated\": \"dflores\", \"applicationName\": \"localhost\" }",
  "httpMethod": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "isBase64Encoded": false,
  "path": "/",
  "queryStringParameters": null
}
{
  "body": "{ \"name\": \"toño\", \"lastame\": \"vargas\", \"email\": \"tvargas@acity.com.pe\", \"phone\": \"935742256\", \"userCreated\": \"dflores\", \"applicationName\": \"localhost\" }",
  "httpMethod": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "isBase64Encoded": false,
  "path": "/",
  "queryStringParameters": null
}

{
  "body": "{ \"name\": \"marina\", \"lastame\": \"padilla\", \"email\": \"mpadilla@acity.com.pe\", \"phone\": \"935742656\", \"userCreated\": \"dflores\", \"applicationName\": \"localhost\" }",
  "httpMethod": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "isBase64Encoded": false,
  "path": "/",
  "queryStringParameters": null
}
