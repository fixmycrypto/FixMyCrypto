# Prerequisites

[.NET (dotnet) 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

# Build & Run from command line:

Clone the repository: 

    git clone https://github.com/fixmycrypto/FixMyCrypto.git

Install dependencies and run:

    cd FixMyCrypto
    dotnet run

# Develop

VS Code project files are included, install "C# for Visual Studio Code (powered by OmniSharp)" extension.

# Debug

Debug Run configuration in VS Code.

# Build release

    dotnet publish -c Release

# Tests

* Unit tests: run Test configuration in VS Code (`FixMyCrypto -t`)
* Unit tests require phrases and addresses specified in `secrets.json` (see `secrets.example.json`)
    * Blockchain search tests require some coins to be deposited in those addresses
* Release tests: Run `./tests.sh`
