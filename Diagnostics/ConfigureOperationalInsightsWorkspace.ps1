    <#
    This script will configure an Operations Management Suite workspace (aka Operational Insights workspace) to read Diagnostics from an Azure Storage account.

    It will enable all supported data types (currently Windows Event Logs, Syslog, Service Fabric Events, ETW Events and IIS Logs).

    It supports both classic and Resource Manager storage accounts.

    If you have more than one OMS workspace you will be prompted for the workspace to configure.

    If you have more than one storage account you will be prompted for which storage account to configure.
    #>

# To login to Azure Resource Manager
Login-AzureRmAccount

# Select a default subscription for your current session in case your account has multiple Azure subscriptions
Get-AzureRmSubscription –SubscriptionName “Paolo's Azure Account” | Select-AzureRmSubscription

# Define variables
$storageAccountResourceGroupName = 'BaboServiceFabricWestEurope'
$storageAccountResourceName = "sfdgbabo7796"
$OperationalInsightsWorkspaceResourceGroup = 'OI-Default-West-Europe'
$OperationalInsightsWorkspaceResourceName = 'babo'
$validTables = "WADServiceFabric*EventTable", "WADETWEventTable", "WADPerformanceCountersTable", "WADWindowsEventLogsTable"
$validContainers = @()


$workspace = Get-AzureRmOperationalInsightsWorkspace -ResourceGroupName $OperationalInsightsWorkspaceResourceGroup -Name $OperationalInsightsWorkspaceResourceName
$storageAccount = Get-AzureRmResource -ResourceGroupName $storageAccountResourceGroupName -ResourceType Microsoft.Storage/storageAccounts -ResourceName $storageAccountResourceName
$insightsName = $workspace.Name + '_' + $storageAccount.Name
$existingConfig = ""

try
{
    $existingConfig = Get-AzureRmOperationalInsightsStorageInsight -Workspace $workspace -Name $insightsName -ErrorAction Stop
}
catch [Hyak.Common.CloudException]
{
    # HTTP Not Found is returned if the storage insight doesn't exist
}

if ($existingConfig) 
{
    Set-AzureRmOperationalInsightsStorageInsight -Workspace $workspace -Name $insightsName -Tables $validTables -Containers $validContainers
} 
else 
{
    $key = (Get-AzureRmStorageAccountKey -ResourceGroupName $storageAccount.ResourceGroupName -Name $storageAccount.Name).Key1
    New-AzureRmOperationalInsightsStorageInsight -Workspace $workspace -Name $insightsName -StorageAccountResourceId $storageAccount.ResourceId -StorageAccountKey $key -Tables $validTables -Containers $validContainers
}