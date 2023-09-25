param(
    [Parameter(Mandatory=$true)]$scriptDir,
    [Parameter(Mandatory=$true)]$angularDir,
    [Parameter(Mandatory=$false)]$repoDir,
    [Parameter(Mandatory=$true)]$netDir,
    [Parameter(Mandatory=$true)]$publishDir,
    [Parameter(Mandatory=$true)]$computerName,
    [Parameter(Mandatory=$true)]$username,
    [Parameter(Mandatory=$true)]$passwordLocation,
    [Parameter(Mandatory=$true)]$sessionAngularDir,
    [Parameter(Mandatory=$true)]$sessionNetDir,
    [Parameter(Mandatory=$true)]$angularSiteName,
    [Parameter(Mandatory=$true)]$netSiteName

)

Write-Output $angularDir
Write-Output $repoDir
Write-Output $netDir
Write-Output $publishDir
Write-Output $computerName
Write-Output $username
Write-Output $passwordLocation
Write-Output $sessionAngularDir
Write-Output $sessionNetDir

cd $scriptDir
.\build_angular.ps1 -angularDir $angularDir

if (-not ([string]::IsNullOrEmpty($repoDir)))
{
    Write-Host "Angular -> Github sync commencing."
    cd $scriptDir
    .\sync_angular_github.ps1 -angularDir $angularDir -repoDir $repoDir
}

cd $scriptDir
.\dotnet_build_publish_prod.ps1 -netDir $netDir -publishDir $publishDir

cd $scriptDir
$vpsSession = .\initialize_ps_remote_session.ps1 -computerName $computerName -username $username -passwordLocation $passwordLocation

cd $scriptDir
.\stop_iis_remote.ps1 -vpsSession $vpsSession

$angularDistDir = $repoDir + "\dist"

.\copy_files_to_vps.ps1 `
-vpsSession $vpsSession `
-angularDistDir $angularDistDir `
-publishDir $publishDir `
-sessionAngularDir $sessionAngularDir `
-sessionNetDir $sessionNetDir

cd $scriptDir
.\iis_set_http_headers.ps1 -vpsSession $vpsSession -siteName $angularSiteName

cd $scriptDir
.\add_angular_url_rewrite_rule.ps1 -vpsSession $vpsSession -siteName $angularSiteName

cd $scriptDir
.\start-iis-remote.ps1 -vpsSession $vpsSession -angularSiteName $angularSiteName -netSiteName $netSiteName

New-BurntToastNotification -AppLogo "C:\Windows\System32\@WindowsHelloFaceToastIcon.png" -Text "Deployment finished."