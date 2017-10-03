#!/bin/bash
rm -r ./publish
set -e
dotnet restore
dotnet build
# dotnet test src/KitchenResponsibleServiceTests/KitchenResponsibleServiceTests.csproj --no-build --no-restore
dotnet publish Hjerpbakk.PoorMansServiceDiscovery/Hjerpbakk.PoorMansServiceDiscovery.csproj -o ../publish -c Release
docker build -t service-discovery .