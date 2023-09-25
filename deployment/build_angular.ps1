#Build Angular
param(
    [Parameter(Mandatory=$true)]$angularDir
)

cd $angularDir
ng build

Write-Host "ng build finished."