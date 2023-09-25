param(
    [Parameter(Mandatory=$true)]$vpsSession,
    [Parameter(Mandatory=$true)]$siteName
)


Import-Module WebAdministration

$block = {
    param($siteName)

    $site = "iis:\sites\$siteName"

    Clear-WebConfiguration -pspath $site -filter '/system.webserver/rewrite/rules/rule'

    Add-WebConfigurationProperty -pspath $site -filter "system.webserver/rewrite/rules" -name "." -value @{name='HTTPS Rewrite'; patternSyntax='Regular Expression'; stopProcessing='True'}
    
    $filterRoot = "system.webserver/rewrite/rules/rule[@name='HTTPS Rewrite']"
    
    Set-WebConfigurationProperty -pspath $site -filter "$filterRoot/match" -name "url" -value "^(.*)$"
    Set-WebConfigurationProperty -pspath $site -filter "$filterRoot/conditions" -name "logicalGrouping" -value "MatchAny"
    Set-WebConfigurationProperty -pspath $site -filter "$filterRoot/action" -name "type" -value "Rewrite"
    Set-WebConfigurationProperty -pspath $site -filter "$filterRoot/action" -name "url" -value "index.html"
    Set-WebConfigurationProperty -pspath $site -filter "$filterRoot/action" -name "appendQueryString" -value "true"
    
    Add-WebConfigurationProperty -pspath $site -filter "$filterRoot/conditions" -name "." -value @{input="{REQUEST_FILENAME}"; matchType="IsFile"; negate="true";}
}

Invoke-Command -Session $vpsSession -ScriptBlock $block -ArgumentList $siteName