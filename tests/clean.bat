@echo off
Rem loop through directories
for /D %%i in (*) do (
    echo Clean test %%i
    pushd %%i

    Rem delete old test results
    del /q results.json log.txt

    popd
)
