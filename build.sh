#!/bin/bash
rm -r ./publish
set -e
dotnet publish Hjerpbakk.PoorMansServiceDiscovery/Hjerpbakk.PoorMansServiceDiscovery.csproj -o ../publish -c Release
docker build -t service-discovery .