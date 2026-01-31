param location string = resourceGroup().location
param namespaceName string = 'ecomm-microservices-dev'

resource sbNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  name: namespaceName
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource orderEventsTopic 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  parent: sbNamespace
  name: 'order-events'
}

resource orderServiceSubscription 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  parent: orderEventsTopic
  name: 'create-order'
  properties: {
    maxDeliveryCount: 5
    deadLetteringOnMessageExpiration: true
  }
}

// // Managed Identities
// resource cartServiceMI 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
//   name: 'mi-cartservice'
//   location: location
// }

// resource orderServiceMI 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
//   name: 'mi-orderservice'
//   location: location
// }

// // Auth. roles
// resource cartSenderRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(sbNamespace.id, cartServiceMI.id, 'sender')
//   scope: sbNamespace
//   properties: {
//     principalId: cartServiceMI.properties.principalId
//     roleDefinitionId: subscriptionResourceId(
//       'Microsoft.Authorization/roleDefinitions',
//       '69a216fc-b8fb-44d8-bc22-1f3c2cd27a39'
//     )
//   }
// }

// resource orderReceiverRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(sbNamespace.id, orderServiceMI.id, 'receiver')
//   scope: sbNamespace
//   properties: {
//     principalId: orderServiceMI.properties.principalId
//     roleDefinitionId: subscriptionResourceId(
//       'Microsoft.Authorization/roleDefinitions',
//       '090c5cfd-751d-490a-894a-3ce6f1109419'
//     )
//   }
// }

output serviceBusNamespaceId string = sbNamespace.id
