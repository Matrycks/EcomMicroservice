param environment string
param image string
param containerEnvId string
param registryUsername string
param registryPassword string

module catalog '../modules/container-app.bicep' = {
  name: 'catalog-app'
  params: {
    appName: 'catalog-api-${environment}'
    containerEnvId: containerEnvId
    image: image
    registryUsername: registryUsername
    registryPassword: registryPassword
    environment: environment
    envVars: [
      { name: 'ASPNETCORE_ENVIRONMENT', value: environment }
    ]
  }
}
