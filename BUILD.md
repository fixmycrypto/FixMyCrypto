# Build:

    dotnet restore
    dotnet build

## Release builds:

    dotnet publish -c Release -r win-x64 --self-contained
    dotnet publish -c Release -r osx-x64 --self-contained
    dotnet publish -c Release -r linux-x64 --self-contained

# Develop

VS Code project files are included. Prereqs: .NET 5.0

# Test:

* run unit tests (-t)
* run tests\tests.bat

# Release build torture test (requires blockchain nodes):

    .\bin\Release\net5.0\win-x64\FixMyCrypto.exe -t 100 ETH,BTC,ADA
