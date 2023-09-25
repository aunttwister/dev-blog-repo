param(
    [Parameter(Mandatory=$true)]$computerName,
    [Parameter(Mandatory=$true)]$username,
    [Parameter(Mandatory=$true)]$passwordLocation
)

$password = Get-Content $passwordLocation | ConvertTo-SecureString -AsPlainText -Force

$cred = new-object -typename System.Management.Automation.PSCredential -argumentlist $username, $password

$options = New-PsSessionOption -SkipCACheck -SkipCNCheck
return New-PsSession -ComputerName $computername -Credential $cred -UseSSL -SessionOption $options