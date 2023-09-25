param(
    [Parameter(Mandatory=$true)]$netDir,
    [Parameter(Mandatory=$true)]$publishDir
)

cd $netDir
dotnet build
dotnet publish --output $publishDir /p:EnvironmentName=Production

Write-Host ".NET build and production publish finished."