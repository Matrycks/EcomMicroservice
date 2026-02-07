param environment string
param image string
param containerEnvId string
param registryUsername string
param registryPassword string
param location string = resourceGroup().location
param serviceBusNamespace string = 'ecomm-microservices-dev'
var senderRole string = '69a216fc-b8fb-44d8-bc22-1f3c2cd27a39'

var receiverRole string = '090c5cfd-751d-490a-894a-3ce6f1109419'

// Managed Identities
resource orderServiceMI 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'mi-orderservice'
  location: location
}

resource sbNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' existing = {
  name: serviceBusNamespace
}

resource ordersTopic 'Microsoft.ServiceBus/namespaces/topics@2024-01-01' existing = {
  name: 'order-events'
  parent: sbNamespace
}

resource orderServiceSubscription 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2024-01-01' existing = {
  name: 'create-order'
  parent: ordersTopic
}

// Auth. roles
resource orderSenderRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(ordersTopic.id, orderServiceMI.id, senderRole)
  scope: ordersTopic
  properties: {
    principalId: orderServiceMI.properties.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', senderRole)
    principalType: 'ServicePrincipal'
  }
}

resource orderReceiverRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(orderServiceSubscription.id, orderServiceMI.id, receiverRole)
  scope: orderServiceSubscription
  properties: {
    principalId: orderServiceMI.properties.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', receiverRole)
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
