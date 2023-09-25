param(
    [Parameter(Mandatory=$true)]$vpsSession,
    [Parameter(Mandatory=$true)]$angularDistDir,
    [Parameter(Mandatory=$true)]$publishDir,
    [Parameter(Mandatory=$true)]$sessionAngularDir,
    [Parameter(Mandatory=$true)]$sessionNetDir
)

#File transfer
Copy-Item -ToSession $vpsSession $angularDistDir\* $sessionAngularDir -Recurse -Force
Copy-Item -ToSession $vpsSession $publishDir\* $sessionNetDir -Recurse -Force