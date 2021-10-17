# Develop

Prereqs: .NET 5.0 SDK

VS Code project files are included, install "C# for Visual Studio Code (powered by OmniSharp)" extension.

# Build & Run from command line:

    dotnet restore
    dotnet build
    dotnet run

# Debug

Debug Run configuration in VS Code.

## Release builds:

    dotnet publish -c Release -r win-x64 --self-contained
    dotnet publish -c Release -r osx-x64 --self-contained
    dotnet publish -c Release -r linux-x64 --self-contained

# Test:

* Unit tests: run Test configuration in VS Code (or run `FixMyCrypto -t`)
* Unit tests require phrases and addresses specified in `secrets.json` (see `secrets.example.json`)
    * Blockchain search tests require some coins to be deposited in those addresses
* Production tests: `cd tests && .\tests.bat`
