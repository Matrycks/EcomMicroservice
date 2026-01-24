param location string = resourceGroup().location
param environment string

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'logs-${environment}'
  location: location
}

resource containerEnv 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: 'containerEnv-${environment}'
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics-${environment}'
      logAnalyticsConfiguration: {
        customerId: logAnalytics.properties.customerId
        sharedKey: logAnalytics.listKeys().primarySharedKey
      }
    }
  }
}

output containerEnvId string = containerEnv.id
