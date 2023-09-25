param(
    [Parameter(Mandatory=$true)]$vpsSession,
    [Parameter(Mandatory=$true)]$angularSiteName,
    [Parameter(Mandatory=$true)]$netSiteName
)

Import-Module WebAdministration
$block = {
    param($netSiteName, $angularSiteName)
    iisreset /start; Start-WebSite $netSiteName; Start-WebSite $angularSiteName;
};
Invoke-Command -Session $vpsSession -ScriptBlock $block -ArgumentList $netSiteName, $angularSiteName