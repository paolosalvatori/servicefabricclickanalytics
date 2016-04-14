$resourceGroupName = 'BaboServiceFabricWestEurope'
$deploymentName = 'IoTDemoWadCfg'

Stop-AzureRmResourceGroupDeployment -Name $deploymentName `
                                   -ResourceGroupName $resourceGroupName 
                                   