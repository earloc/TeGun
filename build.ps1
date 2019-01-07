param (
    [Parameter(Mandatory=$true)][int]$buildNumber,
    [Parameter(Mandatory=$false)]$branch
)

. ".\_Functions.ps1";

$packageSuffix = GetPackageSuffix($branch)

Write-Host $packageSuffix

BuildAndPack "./src" "dotnet-tegun" $packageSuffix

nuget pack "./src/dotnet-tegun.template/dotnet-tegun.template.nuspec" -OutputDirectory "./src/dotnet-tegun.template/bin/packages"