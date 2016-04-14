# To login to Azure Resource Manager
Login-AzureRmAccount

# Select a default subscription for your current session in case your account has multiple Azure subscriptions
Get-AzureRmSubscription –SubscriptionName “Paolo's Azure Account” | Select-AzureRmSubscription

# Initialize Variables
$resourceGroupName = 'BaboServiceFabricWestEurope'
$deploymentName = 'IoTDemoWadCfg'
$templateFile = 'C:\Projects\Azure\ServiceFabric\IoTDemo\Diagnostics\IoTDemoWadConfigXmlUpdate.json'
$templateParameterFile = 'C:\Projects\Azure\ServiceFabric\IoTDemo\Diagnostics\IoTDemoWadConfigUpdateParams.json'

# Check if a resource group with the specified name actually exists
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName `
                                          -ErrorAction Ignore

if ($resourceGroup)
{
    # The resource group exists
    Write-Host 'The resource group' $resourceGroupName ' exists'
}
else
{
    # The resource group does not exist
    Write-Host 'The resource group' $resourceGroupName ' does not exist'
    Return
}

# Check if a deployment with the specified name already exists
$deployment = Get-AzureRmResourceGroupDeployment -Name $deploymentName `
                                                 -ResourceGroupName $resourceGroupName `
                                                 -ErrorAction Ignore

if ($deployment)
{
    # Delete the deployment if it already exists
    Write-Host 'The' $deploymentName 'deployment already exists. Deleting the deployment...'
    $stopWatch = [Diagnostics.Stopwatch]::StartNew()
    Remove-AzureRmResourceGroupDeployment -Name $deploymentName `
                                          -ResourceGroupName $resourceGroupName `
                                          -Force `
                                          -ErrorAction Stop
    $stopWatch.Stop()
    Write-Host 'The deployment' $deploymentName ' has been successfully deleted in ' $stopWatch.Elapsed.TotalSeconds 'seconds'
}

Write-Host 'Adding deployment' $deploymentName 'to' $resourceGroupName 'resource group...'
$stopWatch = [Diagnostics.Stopwatch]::StartNew()
New-AzureRmResourceGroupDeployment -Name $deploymentName `
                                   -ResourceGroupName $resourceGroupName `
                                   -TemplateFile $templateFile `
                                   -TemplateParameterFile $templateParameterFile `
                                   -Verbose `
                                   -ErrorAction Stop

$stopWatch.Stop()
$deployment = Get-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName `
                                                 -Name $deploymentName `
                                                 -ErrorAction Stop
Write-Host 'Operation Complete'
Write-Host '=================='
Write-Host 'Provisioning State :' $deployment[0].ProvisioningState
Write-Host 'Time elapsed :' $stopWatch.Elapsed.TotalSeconds 'seconds'