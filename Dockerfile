FROM microsoft/aspnetcore-build:2 AS builder
WORKDIR /source
COPY ./Hjerpbakk.PoorMansServiceDiscovery.sln .

COPY ./Hjerpbakk.PoorMansServiceDiscovery/*.csproj ./Hjerpbakk.PoorMansServiceDiscovery/
RUN dotnet restore

COPY ./Hjerpbakk.PoorMansServiceDiscovery ./Hjerpbakk.PoorMansServiceDiscovery

RUN dotnet publish "./Hjerpbakk.PoorMansServiceDiscovery/Hjerpbakk.PoorMansServiceDiscovery.csproj" --output "../dist" --configuration Release --no-restore

FROM microsoft/aspnetcore:2
WORKDIR /app
COPY --from=builder /source/dist .
ENTRYPOINT ["dotnet", "Hjerpbakk.PoorMansServiceDiscovery.dll"]