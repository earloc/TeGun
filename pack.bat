@echo off


set PackageSuffix=-pre

pushd src

echo packing
echo PackageSuffix:'%PackageSuffix%'
echo ------------------
dotnet pack -c Release --version-suffix "-pre"
echo packing done
echo __________________
popd