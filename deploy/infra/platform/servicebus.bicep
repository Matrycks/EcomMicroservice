param location string = resourceGroup().location
param namespaceName string = 'ecomm-microservices-dev'

resource sbNamespace 'Microsoft.ServiceBus/namespaces@2024-01-01' = {
  name: namespaceName
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource orderEventsTopic 'Microsoft.ServiceBus/namespaces/topics@2024-01-01' = {
  parent: sbNamespace
  name: 'order-events'
}

resource orderServiceSubscription 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2024-01-01' = {
  parent: orderEventsTopic
  name: 'create-order'
  properties: {
    maxDeliveryCount: 5
    deadLetteringOnMessageExpiration: true
  }
}

resource filterRule 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2024-01-01' = {
  name: 'CreateOrderFilter'
  parent: orderServiceSubscription
  properties: {
    filterType: 'SqlFilter'
    sqlFilter: {
      sqlExpression: 'sys.Label = \'CreateOrderMessage\''
    }
  }
}

output serviceBusNamespaceId string = sbNamespace.id
