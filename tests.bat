@echo off
setlocal EnableDelayedExpansion

echo Build Release
dotnet build -c Release
if  !ERRORLEVEL! NEQ 0 (
    echo Build Release failed with $?
    exit /b 1
)

pushd tests

Rem loop through directories
for /D %%i in (*) do (
    echo Run test %%i
    pushd %%i

    Rem delete old test results
    del /q results.json log.txt

    Rem run program
    ..\..\bin\Release\net6.0\FixMyCrypto -ni >> log.txt
    if !ERRORLEVEL! NEQ 0 (
        echo Test %%i failed FixMyCrypto with !ERRORLEVEL!
        exit /b !ERRORLEVEL!
    )
    if NOT EXIST results.json (
        echo Test %%i failed: No results.json
        exit /b 1
    )

    Rem check results
    fc /L results.json expect.json >> log.txt
    if !ERRORLEVEL! NEQ 0 (
        echo Test %%i failed compare with !ERRORLEVEL!
        exit /b !ERRORLEVEL!
    )

    popd
)

popd
