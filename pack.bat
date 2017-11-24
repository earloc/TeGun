@echo off


set PacakgeSuffix=-pre

pushd src

echo packing
echo PacakgeSuffix:'%PacakgeSuffix%'
echo ------------------
dotnet pack -c Release --version-suffix "-pre"
echo packing done
echo __________________
popd