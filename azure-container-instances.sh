#!/bin/bash
set -e

# Needs: brew install azure-cli
# Push to Azure Container Registry
# az group create --name kitchen-responsible-rg --location westeurope
# az acr create --name dipsbot --resource-group kitchen-responsible-rg --admin-enabled --sku Basic
az acr login --name dipsbot
# az acr list --resource-group kitchen-responsible-rg --query "[].{acrLoginServer:loginServer}" --output table
# -> dipsbot.azurecr.io
docker tag service-discovery dipsbot.azurecr.io/service-discovery

# Run container locally
# docker run -p 5000:80 service-discovery
docker push dipsbot.azurecr.io/service-discovery

# Run in Azure Container Instances
# az acr show --name dipsbot --query loginServer
# az acr credential show --name dipsbot --query "passwords[0].value"
az container delete --name service-discovery-service --resource-group kitchen-responsible-rg --yes
az container create --name service-discovery-service --image dipsbot.azurecr.io/service-discovery --cpu 1 --memory 1 --registry-password $AZUREPW --ip-address public -g kitchen-responsible-rg

container_status=$(az container show --name service-discovery-service --resource-group kitchen-responsible-rg --query state)
echo $container_status
while [ $container_status != "\"Running\"" ]
do 
    sleep 5
    container_status=$(az container show --name service-discovery-service --resource-group kitchen-responsible-rg --query state)
    echo $container_status
done

# Uploud IP to Blob Storage
touch ./service-discovery-service.txt
# Needs. AZURE_STORAGE_CONNECTION_STRING environment variable
az container show --name service-discovery-service --resource-group kitchen-responsible-rg --query ipAddress.ip > ./service-discovery-service.txt
cat ./service-discovery-service.txt
az storage blob upload --container-name discovery --file service-discovery-service.txt --name service-discovery-service.txt
