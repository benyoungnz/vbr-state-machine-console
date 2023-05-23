# VBR State Machine

## Getting started
- Make sure you have the .NET 7 SDK installed
- Navigate to the ./vbr-state-machine-console folder
- In a text editor, rename appsettings.json.example to appsettins.json
- Make the necessary changes to the appsettings (see below)
- Open a terminate in ./vbr-state-machine-console folder, type **dotnet run**


## Configuration Options
## Backup Servers
This is an array, you can specify one more backup servers in this array. There is an example in there - other than the hostname, username and password you can likely leave the rest as default unless you have specifically changed items.

## Desired States
There are a number of configuration options here covering most common configuration options spanning several different aspects of Veeam Backup and Replication.

### SOBRPerformanceTier
Desired state for the performance tier of a Scale out backup repository, most settings should be self explanatory

### SOBRPlacementPolicy
Desired state for the placement policy of a Scale out backup repository, most settings should be self explanatory

### SOBRCapacityTier
Desired state for the capacity tier , most settings should be self explanatory however a couple of notes
- MoveAfterDays defines when a restore point should be moved to the capacity tier
- ImmediateCopyRequired is the setting relating to ensure all copies reside on the capacity tier, not just when MoveAfterDays is reached

### GeneralOptions
Desired state for the general options (global) for the Veeam Backup server. , most settings should be self explanatory and align with what you see in the user interface under settings.

### JobConfig
Desired state for the job configuration, most settings should be self explanatory however a couple of notes
- There is an exclusions array, you can elect to put the job names of jobs you wish to exclude from these configuration checks

## Monitors
Currently implented are a few monitoring points - not designed as a replacement for your own monitoring however in some scenarios these may be able to supplement your monitoring solution. For example the Capacity Tier object storage usage, could be hard to monitor through traditional monitoring tools.

### SobrPerformanceTier
Monitors utilisation of each performance extent. The application will calcuate the utilised space as a percent and you can set the warning and critical thresholds. 

### SobrCapacityTier
Monitors utilisation of the connected capacity tier. These values are defined as gigabytes. You can set the warning and critical usage values to warn you when you reach these usage values.

## Alerts
There are currently three (two you will really use 🎅) destinations you can configure.
- **Teams**, used to push State comparision summaries
- **xMatters**, on call paging system which will receive the Monitoring alerts when monitors reach the critical or warning thresholds







