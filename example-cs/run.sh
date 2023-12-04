#!/bin/bash

set -e

if [[ $2 == "rel" ]] || [[ $2 == "test-rel" ]]; then
  echo "### RELEASE BUILD ###"
  build=Release
else
  build=Debug
fi
dotnet build -c $build

echo
export part=$1
if [[ $2 == "test" ]] || [[ $2 == "test-rel" ]]; then
  echo "### TEST MODE ###"
  input=input-test.txt
else
  input=input.txt
fi
time "./bin/$build/net8.0/Aoc" "$input"
