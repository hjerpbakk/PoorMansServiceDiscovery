#!/bin/bash
set -e

container_registry_name="dipscontainerregistry"
container_name="service-discovery"
tagged_container_name="dipsbot.azurecr.io/"$container_name
service_name=$container_name"-service"

# Needs: brew install azure-cli
# Uses current az login, run az logout and az login again if changing directories
# Push to Azure Container Registry
az acr login --name $container_registry_name
docker tag $container_name $tagged_container_name
docker push $tagged_container_name
