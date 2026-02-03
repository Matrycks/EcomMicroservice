param environment string
param image string
param containerEnvId string
param registryUsername string
param registryPassword string
param location string = resourceGroup().location
param serviceBusNamespace string = 'ecomm-microservices-dev'

// Managed Identities
resource catalogServiceMI 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'mi-catalogservice'
  location: location
}

resource sbNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' existing = {
  name: serviceBusNamespace
}

// Auth. roles
resource catalogSenderRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(sbNamespace.id, catalogServiceMI.id, 'sender')
  scope: sbNamespace
  properties: {
    principalId: catalogServiceMI.properties.principalId
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '69a216fc-b8fb-44d8-bc22-1f3c2cd27a39'
    )
  }
}

module catalog '../modules/container-app.bicep' = {
  name: 'catalog-app'
  params: {
    appName: 'catalog-api-${environment}'
    containerEnvId: containerEnvId
    image: image
    registryUsername: registryUsername
    registryPassword: registryPassword
    environment: environment
    identity: {
      type: 'UserAssigned'
      userAssignedIdentities: {
        '${catalogServiceMI.id}': {}
      }
    }
    envVars: [
      { name: 'ASPNETCORE_ENVIRONMENT', value: environment }
      { name: 'SERVICEBUS_NAMESPACE', value: '${serviceBusNamespace}.servicebus.windows.net' }
    ]
  }
}
