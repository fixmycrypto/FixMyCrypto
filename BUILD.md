# Prerequisites

[.NET (dotnet) 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)

# Build & Run from command line:

Clone the repository: 

    git clone --recurse-submodules https://github.com/fixmycrypto/FixMyCrypto.git

Install dependencies and run:

    cd FixMyCrypto
    dotnet run

# Develop

VS Code project files are included, install recommended workspace extensions.

# Debug

Debug Run configuration in VS Code.

# Build release

    dotnet publish -c Release

# Tests

* Unit tests: run Test configuration in VS Code (`FixMyCrypto -t`)
* Unit tests require phrases and addresses specified in `secrets.json` (see example in /examples)
* Release tests: Run `./tests.sh`
