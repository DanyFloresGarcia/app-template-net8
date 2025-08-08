$projectOutput = "./publish"
$lambdaOutput = "publish\lambda-output"
$layerOutput = "publish\layer-output"

Write-Host "========== Generando carpeta publish =========="
dotnet publish -c Release -r linux-x64 --self-contained false -o $projectOutput

Write-Host "========== Generando carpetas de salida =========="
New-Item -ItemType Directory -Force -Path $lambdaOutput | Out-Null
New-Item -ItemType Directory -Force -Path $layerOutput | Out-Null

Write-Host "========== Generando Lista de dlls para Layer =========="
$layerDlls = @(
    "MediatR.dll",
    "MediatR.Contracts.dll",
    "AutoMapper.dll",
    "Azure.Core.dll",
    "Azure.Identity.dll",
    "Dapper.dll",
    "DnsClient.dll",
    "Domain.dll",
    "Domain.pdb",
    "EFCore.NamingConventions.dll",
    "FluentValidation.AspNetCore.dll",
    "FluentValidation.DependencyInjectionExtensions.dll",
    "FluentValidation.dll",
    "Microsoft.Bcl.AsyncInterfaces.dll",
    "Microsoft.Data.SqlClient.dll",
    "Npgsql.dll",
    "Npgsql.EntityFrameworkCore.PostgreSQL.dll",
    "Microsoft.EntityFrameworkCore.Abstractions.dll",
    "Microsoft.EntityFrameworkCore.dll",
    "Microsoft.EntityFrameworkCore.Relational.dll",
    "Microsoft.EntityFrameworkCore.SqlServer.dll",
    "Microsoft.Identity.Client.dll",
    "Microsoft.Identity.Client.Extensions.Msal.dll",
    "Microsoft.IdentityModel.Abstractions.dll",
    "Microsoft.IdentityModel.JsonWebTokens.dll",
    "Microsoft.IdentityModel.Logging.dll",
    "Microsoft.IdentityModel.Protocols.dll",
    "Microsoft.IdentityModel.Protocols.OpenIdConnect.dll",
    "Microsoft.IdentityModel.Tokens.dll",
    "Microsoft.SqlServer.Server.dll",
    "MongoDB.Bson.dll",
    "MongoDB.Driver.dll",
    "MongoDB.EntityFrameworkCore.dll",
    "MySqlConnector.dll",
    "Oracle.ManagedDataAccess.dll",
    "Pomelo.EntityFrameworkCore.MySql.dll",
    "Serilog.AspNetCore.dll",
    "Serilog.dll",
    "Serilog.Extensions.Hosting.dll",
    "Serilog.Extensions.Logging.dll",
    "Serilog.Formatting.Compact.dll",
    "Serilog.Settings.Configuration.dll",
    "Serilog.Sinks.Console.dll",
    "Serilog.Sinks.Debug.dll",
    "Serilog.Sinks.File.dll",
    "SharpCompress.dll",
    "Snappier.dll",
    "SnapshotRestore.Registry.dll",
    "ZstdSharp.dll",
    "System.Configuration.ConfigurationManager.dll",    
    "System.Diagnostics.PerformanceCounter.dll",
    "System.DirectoryServices.Protocols.dll",
    "System.Security.Cryptography.Pkcs.dll"
)

Write-Host "========== Copiando archivos de Layer =========="
foreach ($file in $layerDlls) {
    $source = Join-Path $projectOutput $file
    $destination = Join-Path $layerOutput $file
    if (Test-Path $source) {
        Copy-Item $source $destination -Force
    } else {
        Write-Warning "Archivo no encontrado: $file"
    }
}

Write-Host "========== Copiando archivos de Lambda =========="
Get-ChildItem -Path $projectOutput -Recurse -Include *.dll, *.json -File | ForEach-Object {
    if ($_.Extension -eq ".json" -or ($_.Extension -eq ".dll" -and $layerDlls -notcontains $_.Name)) {
        Write-Host "Copiando a Lambda: $($_.Name)"
        Copy-Item $_.FullName -Destination $lambdaOutput -Force
    }
}


Write-Host "========== Creando archivos .zip de salida =========="
$lambdaZip = "lambda.zip"
$layerZip = "layer.zip"

if (Test-Path $lambdaZip) { Remove-Item $lambdaZip -Force }
if (Test-Path $layerZip) { Remove-Item $layerZip -Force }

Compress-Archive -Path "$lambdaOutput/*" -DestinationPath $lambdaZip
Compress-Archive -Path "$layerOutput/*" -DestinationPath $layerZip

Write-Host "========== ZIPs generados correctamente =========="
Write-Host "- $lambdaZip"
Write-Host "- $layerZip"

Write-Host "========== Limpiando carpetas temporales =========="
Remove-Item -Recurse -Force $lambdaOutput
Remove-Item -Recurse -Force $layerOutput