@echo off

pushd src

dotnet --version
echo restoring
echo ~~~~~~~~~~~~~~~~~~
dotnet restore
echo restoring done
echo __________________
echo building
echo ##################
dotnet build -c Release
echo building done
echo __________________

popd

call pack.bat