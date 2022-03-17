#!/bin/bash

echo Build Release
if ! dotnet build -c Release; then
    echo Build Release failed with $?
    exit $?
fi

pushd tests > /dev/null 2>&1

export DYLD_LIBRARY_PATH=/System/Library/Frameworks/OpenCL.framework

for d in */ ; do
    echo Run test $d

    pushd $d > /dev/null 2>&1 

    rm -f results.json checkpoint.json log.txt graph.dot

    if ! time -p ../../bin/Release/net6.0/FixMyCrypto -ni >> log.txt; then
        echo Test $d failed FixMyCrypto with $?
        exit $?
    fi

    if [ ! -f results.json ]; then
        echo Test $d failed: results.json not found
        exit 1
    fi

    if ! diff -w results.json expect.json >> log.txt; then
        echo Test $d failed: diff returned $?
        exit $?
    fi

    popd > /dev/null 2>&1 
done

popd > /dev/null 2>&1
