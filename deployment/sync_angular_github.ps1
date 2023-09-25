#Angular -> Github sync
param (
    [Parameter(Mandatory=$true)]$angularDir,
    [Parameter(Mandatory=$true)]$repoDir
)

New-Item -ItemType Directory -Force -Path $angularDir
Copy-Item $angularDir\* $repoDir -Exclude @("node_modules",".angular") -Recurse -Force

Write-Host "Sync completed."