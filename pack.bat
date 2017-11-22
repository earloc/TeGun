pushd src

dotnet pack -c Release --version-suffix "-pre"

popd