param(
    [Parameter(Mandatory=$true)]$vpsSession
)

##Stop IIS sites
Import-Module WebAdministration
$block = {iisreset /stop;}  
Invoke-Command -Session $vpsSession -ScriptBlock $block