param appName string
param containerEnvId string
param image string
param environment string
param cpu string = '0.5'
param memory string = '1Gi'
param minReplicas int = 1
param maxReplicas int = 5
param envVars array = []
param registryUsername string
param registryPassword string

resource app 'Microsoft.App/containerApps@2023-05-01' = {
  name: appName
  location: resourceGroup().location
  properties: {
    environmentId: containerEnvId
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
      }
      secrets: [
        {
          name: 'registry-password'
          value: registryPassword
        }
      ]
      registries: [
        {
          server: 'ghcr.io'
          username: registryUsername
          passwordSecretRef: 'registry-password'
        }
      ]
    }
    template: {
      containers: [
        {
          name: appName
          image: image
          resources: {
            cpu: cpu
            memory: memory
          }
          env: envVars
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: maxReplicas
      }
    }
  }
}

output fqdn string = app.properties.configuration.ingress.fqdn
