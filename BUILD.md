# Develop

VS Code project files are included. Prereqs: .NET 5.0

# Build:

    dotnet restore
    dotnet build

## Release builds:

    dotnet publish -c Release -r win-x64 --self-contained
    dotnet publish -c Release -r osx-x64 --self-contained
    dotnet publish -c Release -r linux-x64 --self-contained

# Test:

* run Test configuration in VS Code (or run FixMyCrypto -t)
* cd tests && tests\tests.bat
