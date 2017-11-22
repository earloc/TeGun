pushd src

dotnet restore
dotnet build -c Release

popd

call pack.bat