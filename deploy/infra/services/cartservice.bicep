param environment string
param image string
param containerEnvId string
param registryUsername string
param registryPassword string
param location string = resourceGroup().location
param serviceBusNamespace string
var senderRole string = '69a216fc-b8fb-44d8-bc22-1f3c2cd27a39'

resource cartServiceMI 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'mi-cartservice'
  location: location
}

resource sbNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' existing = {
  name: serviceBusNamespace
}

resource ordersTopic 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' existing = {
  name: 'order-events'
  parent: sbNamespace
}

resource auth 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(ordersTopic.id, cartServiceMI.id, senderRole)
  scope: ordersTopic
  properties: {
    principalId: cartServiceMI.properties.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', senderRole)
    principalType: 'ServicePrincipal'
  }
}

module cartApp '../modules/container-app.bicep' = {
  name: 'cart-app'
  params: {
    appName: 'cart-api-${environment}'
    environment: environment
    containerEnvId: containerEnvId
    image: image
    registryUsername: registryUsername
    registryPassword: registryPassword
    identity: {
      type: 'UserAssigned'
      userAssignedIdentities: {
        '${cartServiceMI.id}': {}
      }
    }
    envVars: [
      { name: 'ASPNETCORE_ENVIRONMENT', value: environment }
      { name: 'SERVICEBUS_NAMESPACE', value: '${serviceBusNamespace}.servicebus.windows.net' }
    ]
  }
}
