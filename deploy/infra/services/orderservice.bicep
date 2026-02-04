param environment string
param image string
param containerEnvId string
param registryUsername string
param registryPassword string
param location string = resourceGroup().location
param serviceBusNamespace string = 'ecomm-microservices-dev'

// Managed Identities
resource orderServiceMI 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'mi-orderservice'
  location: location
}

resource sbNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' existing = {
  name: serviceBusNamespace
}

// Auth. roles
resource orderSenderRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(sbNamespace.id, orderServiceMI.id, 'sender')
  scope: sbNamespace
  properties: {
    principalId: orderServiceMI.properties.principalId
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '69a216fc-b8fb-44d8-bc22-1f3c2cd27a39'
    )
    principalType: 'ServicePrincipal'
  }
}

resource orderReceiverRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(sbNamespace.id, orderServiceMI.id, 'receiver')
  scope: sbNamespace
  properties: {
    principalId: orderServiceMI.properties.principalId
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '090c5cfd-751d-490a-894a-3ce6f1109419'
    )
    principalType: 'ServicePrincipal'
  }
}

module order '../modules/container-app.bicep' = {
  name: 'order-app'
  params: {
    appName: 'order-api-${environment}'
    containerEnvId: containerEnvId
    image: image
    registryUsername: registryUsername
    registryPassword: registryPassword
    environment: environment
    identity: {
      type: 'UserAssigned'
      userAssignedIdentities: {
        '${orderServiceMI.id}': {}
      }
    }
    envVars: [
      { name: 'ASPNETCORE_ENVIRONMENT', value: environment }
      { name: 'SERVICEBUS_NAMESPACE', value: '${serviceBusNamespace}.servicebus.windows.net' }
    ]
  }
}
