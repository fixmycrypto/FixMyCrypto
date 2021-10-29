# Prerequisites: 

.NET 6.0 SDK

# Build & Run from command line:

    dotnet run

# Develop

VS Code project files are included, install "C# for Visual Studio Code (powered by OmniSharp)" extension.

# Debug

Debug Run configuration in VS Code.

# Release builds:

    dotnet publish -c Release --self-contained

# Test:

* Unit tests: run Test configuration in VS Code (or run `FixMyCrypto -t`)
* Unit tests require phrases and addresses specified in `secrets.json` (see `secrets.example.json`)
    * Blockchain search tests require some coins to be deposited in those addresses
* Release tests: `./tests.sh`
