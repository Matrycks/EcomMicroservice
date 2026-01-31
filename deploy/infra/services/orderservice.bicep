param environment string
param image string
param containerEnvId string
param registryUsername string
param registryPassword string
param serviceBusNamespace string = 'ecomm-microservices-dev'

module order '../modules/container-app.bicep' = {
  name: 'order-app'
  params: {
    appName: 'order-api-${environment}'
    containerEnvId: containerEnvId
    image: image
    registryUsername: registryUsername
    registryPassword: registryPassword
    environment: environment
    envVars: [
      { name: 'ASPNETCORE_ENVIRONMENT', value: environment }
      { name: 'SERVICEBUS_NAMESPACE', value: '${serviceBusNamespace}.servicebus.windows.net' }
    ]
  }
}
