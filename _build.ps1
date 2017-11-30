
Function GetPackageSuffix() {
    param (
        [string]$branch
    )

    if ($branch -eq 'master') {
        return ''
    }
    
    if ($branch -ne '') {
        return "-$branch"
    }

    return ''
}

Function BuildAndPack() {
    param (
        [Parameter(Mandatory=$true)]$baseDirectory, 
        [Parameter(Mandatory=$true)]$projectName, 
        [Parameter(Mandatory=$true)]$packageSuffix
    )

    $directory = "$baseDirectory\$projectName"
    $projectFile = "$directory\$projectName.csproj"

    $patchedVersion = PatchVersion $directory $projectFile $packageSuffix

    $assemblyVersion = $patchedVersion.Assembly
    $packageVersion = $patchedVersion.Package
    

    Write-Host "-------------------------------------------------------"
    Write-Host "Building $project with version $assemblyVersion"
    Write-Host "-------------------------------------------------------"
    dotnet build $projectFile /p:Version=$assemblyVersion

    Write-Host "-------------------------------------------------------"
    Write-Host "packing $project with package version $packageVersion"
    Write-Host "-------------------------------------------------------"
    dotnet pack  $projectFile /p:Version=$packageVersion --no-build
}

Function PatchVersion() {

    param(
        [Parameter(Mandatory=$true)]
        $projectDirectory, 
        [Parameter(Mandatory=$true)]
        $projectFile,
        [Parameter(Mandatory=$true)]
        $packageSuffix
    )
    
Write-Host $projectDirectory $projectFile $packageSuffix

    $patchedVersion = "" | Select-Object -Property Assembly,Package
        
    [xml]$projectXml = Get-Content $projectFile
    $version = Select-Xml "//Version" $projectXml | ForEach-Object {$_.Node.'#text'}
    
    $packageOverridesFile = "$directory\Package.Overrides.xml"
    
    if (Test-Path $packageOverridesFile ) {
        [xml]$overridesXml = Get-Content $packageOverridesFile
        $packageSuffixOverride = Select-Xml "/Package/Suffix" $overridesXml | ForEach-Object {$_.Node.'#text'}
    
        if ($packageSuffixOverride -ne '') {
            Write-Host "-------------------------------------------------------"
            Write-Host "overriding package suffix to: '$packageSuffixOverride'"
            Write-Host "-------------------------------------------------------"
            $packageSuffix = "-$packageSuffixOverride"
        }
    }
    
    $patchedVersion.Assembly = "$version.$buildNumber"
    $patchedVersion.Package = "$version.$buildNumber$packageSuffix"

    return $patchedVersion
}

