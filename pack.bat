@echo off


set PackageSuffix=-pre

pushd src

echo packing
echo PackageSuffix:'%PackageSuffix%'
echo ------------------
dotnet pack -c Release --version-suffix "-pre"
echo packing done

echo packing template
nuget pack dotnet-tegun.template\dotnet-tegun.template.nuspec -OutputDirectory dotnet-tegun.template\bin\packages
echo done packing template
echo __________________
popd