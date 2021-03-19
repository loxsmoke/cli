pushd ..\src
rmdir /q /s bin\release\netcoreapp3.1
dotnet build -c release
pushd bin\release\netcoreapp3.1
mddox cli.dll -t CliDefinitionList -o ..\..\..\..\clidefinition.md -i "CLI data model reference" -a JsonIgnore -m
popd
popd
