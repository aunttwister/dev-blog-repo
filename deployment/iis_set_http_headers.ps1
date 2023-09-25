param(
    [Parameter(Mandatory=$true)]$vpsSession,
    [Parameter(Mandatory=$true)]$siteName
)

 
Import-Module IISAdministration
Import-Module WebAdministration
 
$block = {
    param($siteName)
    $CustomHeaders = @(
    [pscustomobject]@{Name="Access-Control-Allow-Origin"; Value="*"}
    )

    $CustomHeaders | ForEach-Object {
    $IISConfigSection = Get-IISConfigSection -SectionPath system.webServer/httpProtocol -CommitPath $siteName | Get-IISConfigCollection -CollectionName "customHeaders";
    $Header = Get-IISConfigCollectionElement -ConfigCollection $IISConfigSection -ConfigAttribute @{ 'name' = $_.Name }
    if(!$Header){
        New-IISConfigCollectionElement -ConfigCollection $IISConfigSection -ConfigAttribute @{"name"=$_.Name; "value"=$_.Value;};
        $IIS = Get-IISServerManager
        $IIS.CommitChanges();
    }
};}

Invoke-Command -Session $vpsSession -ScriptBlock $block -ArgumentList $siteName