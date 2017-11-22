@echo off

pushd src

echo packing
echo ------------------
dotnet pack -c Release --version-suffix "-pre"
echo packing done
echo __________________
popd