@echo off


set PackageSuffix=-pre

pushd src

echo packing
echo PackageSuffix:'%PackageSuffix%'
echo ------------------
dotnet pack -c Release --version-suffix "-pre"
echo packing done

echo packing template
nuget pack src\dotnet-tegun.template\dotnet-tegun.template.nuspec -OutputDirectory src\dotnet-tegun.template\bin\packages
echo done packing template
echo __________________
popd