using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using vbr_state_machine_console.Models.VBR;

namespace vbr_state_machine_console.Models.VBR.Job
{    
    // Job Configuration
    public partial class Configurations
    {
        [JsonProperty("data")]
        public List<Configuration> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public partial class Configuration
    {
        [JsonProperty("isHighPriority")]
        public bool IsHighPriority { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("virtualMachines")]
        public VirtualMachines VirtualMachines { get; set; }

        [JsonProperty("storage")]
        public Storage Storage { get; set; }

        [JsonProperty("guestProcessing")]
        public GuestProcessing GuestProcessing { get; set; }

        [JsonProperty("schedule")]
        public Schedule Schedule { get; set; }
    }

    public partial class VirtualMachines
    {
        [JsonProperty("includes")]
        public List<Includes> Includes { get; set; }

        [JsonProperty("excludes")]
        public Excludes? Excludes { get; set; }
    }

    public partial class Includes
    {
        [JsonProperty("inventoryObject")]
        public InventoryObject InventoryObject { get; set; }

        [JsonProperty("size")]
        public string? Size { get; set; }
    }

    public partial class Excludes
    {
        [JsonProperty("vms")]
        public List<ExcludeVM>? VMs { get; set; }

        [JsonProperty("disks")]
        public List<Disk>? Disks { get; set; }

        [JsonProperty("templates")]
        public Templates? Templates { get; set; }
    }

    public partial class ExcludeVM
    {
        [JsonProperty("inventoryObject")]
        public InventoryObject InventoryObject { get; set; }

        [JsonProperty("size")]
        public string? Size { get; set; }
    }

    public partial class InventoryObject
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("hostName")]
        public string HostName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("objectId")]
        public string? ObjectId { get; set; }
    }

    public partial class Disk
    {
        [JsonProperty("disksToProcess")]
        public string DisksToProcess { get; set; }

        [JsonProperty("vmObject")]
        public InventoryObject VMObject { get; set; }

        [JsonProperty("disks")]
        public List<string> Disks { get; set; }

        [JsonProperty("removeFromVMConfiguration")]
        public bool? RemoveFromVmConfiguration { get; set; }
    }    

    public partial class Templates
    {
        [JsonProperty("isEnabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("excludeFromIncremental")]
        public bool? ExcludeFromIncremental { get; set; }
    }

    public partial class Storage
    {
        [JsonProperty("backupRepositoryId")]
        public string BackupRepositoryId { get; set; }

        [JsonProperty("backupProxies")]
        public BackupProxies BackupProxies { get; set; }

        [JsonProperty("retentionPolicy")]
        public RetentionPolicy RetentionPolicy { get; set; }

        [JsonProperty("gfsPolicy")]
        public GfsPolicy? GfsPolicy { get; set; }

        [JsonProperty("advancedSettings")]
        public AdvancedSettings? AdvancedSettings { get; set; }
    }

    public partial class BackupProxies
    {
        [JsonProperty("autoSelection")]
        public bool AutoSelection { get; set; }

        [JsonProperty("proxyIds")]
        public List<Guid>? ProxyIds { get; set; }
    }

    public partial class RetentionPolicy
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }
    }

    public partial class GfsPolicy
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("weekly")]
        public GfsWeekly? Weekly { get; set; }

        [JsonProperty("monthly")]
        public GfsMonthly? Monthly { get; set; }

        [JsonProperty("yearly")]
        public GfsYearly? Yearly { get; set; }
    }

    public partial class GfsWeekly
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("keepForNumberOfWeeks")]
        public double? KeepForNumberOfWeeks { get; set; }

        [JsonProperty("desiredTime")]
        public string? DesiredTime { get; set; }
    }

    public partial class GfsMonthly
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("keepForNumberOfMonths")]
        public double? KeepForNumberOfMonths { get; set; }

        [JsonProperty("desiredTime")]
        public string? DesiredTime { get; set; }
    }

    public partial class GfsYearly
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("keepForNumberOfYears")]
        public double? KeepForNumberOfYears { get; set; }

        [JsonProperty("desiredTime")]
        public string? DesiredTime { get; set; }
    }

    public partial class AdvancedSettings
    {
        [JsonProperty("backupModeType")]
        public string BackupModeType { get; set; }

        [JsonProperty("synthenticFulls")]
        public SynthenticFulls? SynthenticFulls { get; set; }

        [JsonProperty("activeFulls")]
        public ActiveFulls? ActiveFulls { get; set; }

        [JsonProperty("backupHealth")]
        public BackupHealth? BackupHealth { get; set; }

        [JsonProperty("fullBackupMaintenance")]
        public FullBackupMaintenance? FullBackupMaintenance { get; set; }

        [JsonProperty("storageData")]
        public StorageData? StorageData { get; set; }

        [JsonProperty("notifications")]
        public Notifications? Notifications { get; set; }

        [JsonProperty("vSphere")]
        public VSphere? VSphere { get; set; }

        [JsonProperty("storageIntegration")]
        public StorageIntegration? StorageIntegration { get; set; }

        [JsonProperty("scripts")]
        public JobScripts? Scripts { get; set; }
    }

    public partial class Notifications
    {
        [JsonProperty("sendSNMPNotifications")]
        public bool? SendSnmpNotifications { get; set; }

        [JsonProperty("emailNotifications")]
        public EmailNotifications? EmailNotifications { get; set; }

        [JsonProperty("vmAttribute")]
        public VmAttribute? VmAttribute { get; set; }
    }

    public partial class EmailNotifications
    {
        [JsonProperty("notificationType")]
        public string? NotificationType { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("recipients")]
        public List<string>? Recipients { get; set; }

        [JsonProperty("customNotificationSettings")]
        public CustomNotificationSettings? CustomNotificationSettings { get; set; }
    }

    public partial class CustomNotificationSettings
    {
        [JsonProperty("subject")]
        public string? Subject { get; set; }
        
        [JsonProperty("notifyOnSuccess")]
        public bool? NotifyOnSuccess { get; set; }

        [JsonProperty("notifyOnWarning")]
        public bool? NotifyOnWarning { get; set; }

        [JsonProperty("notifyOnError")]
        public bool? NotifyOnError { get; set; }

        [JsonProperty("SuppressNotificationUntilLastRetry")]
        public bool? SuppressNotificationUntilLastRetry { get; set; }
    }

    public partial class VmAttribute
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("appendToExisitingValue")]
        public bool AppendToExisitingValue { get; set; }
    }

    public partial class VSphere
    {
        [JsonProperty("enableVMWareToolsQuiescence")]
        public bool? EnableVmwareToolsQuiescence { get; set; }

        [JsonProperty("changedBlockTracking")]
        public ChangedBlockTracking? ChangedBlockTracking { get; set; }
    }

    public partial class ChangedBlockTracking
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("enableCBTautomatically")]
        public bool? EnableCbtautomatically { get; set; }

        [JsonProperty("resetCBTonActiveFull")]
        public bool? ResetCbtonActiveFull { get; set; }
    }

    public partial class StorageData
    {
        [JsonProperty("compressionLevel")]
        public string? CompressionLevel { get; set; }

        [JsonProperty("storageOptimization")]
        public string? StorageOptimization { get; set; }

        [JsonProperty("enableInlineDataDedup")]
        public bool? EnableInlineDataDedup { get; set; }

        [JsonProperty("excludeSwapFileBlocks")]
        public bool? ExcludeSwapFileBlocks { get; set; }

        [JsonProperty("excludeDeletedFileBlocks")]
        public bool? ExcludeDeletedFileBlocks { get; set; }

        [JsonProperty("encryption")]
        public Encryption? Encryption { get; set; }
    }

    public partial class Encryption
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("encryptionPasswordIdOrNull")]
        public Guid? EncryptionPasswordIdOrNull { get; set; }

        [JsonProperty("encryptionPasswordTag")]
        public string? EncryptionPasswordTag { get; set; }
    }

    public partial class SynthenticFulls
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("weekly")]
        public Weekly? Weekly { get; set; }

        [JsonProperty("monthly")]
        public Monthly? Monthly { get; set; }
    }

    public partial class StorageIntegration
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("limitProcessedVm")]
        public bool? LimitProcessedVm { get; set; }

        [JsonProperty("limitProcessedVmCount")]
        public double? LimitProcessedVmCount { get; set; }

        [JsonProperty("failoverToStandardBackup")]
        public bool? FailoverToStandardBackup { get; set; }
    }

    public partial class ActiveFulls
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("weekly")]
        public Weekly? Weekly { get; set; }

        [JsonProperty("monthly")]
        public Monthly? Monthly { get; set; }
    }

    public partial class Weekly
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("days")]
        public List<string>? Days { get; set; }
    }

    public partial class Monthly
    {
        [JsonProperty("dayOfWeek")]
        public string? DayOfWeek { get; set; }

        [JsonProperty("dayNumberInMonth")]
        public string? DayNumberInMonth { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("localTime")]
        public DateTime? LocalTime { get; set; }

        [JsonProperty("dayOfMonth")]
        public double? DayOfMonth { get; set; }

        [JsonProperty("months")]
        public List<string>? Months { get; set; }
    }

    public partial class BackupHealth
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("weekly")]
        public Weekly? Weekly { get; set; }

        [JsonProperty("monthly")]
        public Monthly? Monthly { get; set; }
    }

    public partial class FullBackupMaintenance
    {
        [JsonProperty("RemoveData")]
        public RemoveData? RemoveData { get; set; }

        [JsonProperty("defragmentAndCompact")]
        public DefragmentAndCompact? DefragmentAndCompact { get; set; }
    }

    public partial class DefragmentAndCompact
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("weekly")]
        public Weekly? Weekly { get; set; }

        [JsonProperty("monthly")]
        public Monthly? Monthly { get; set; }
    }

    public partial class RemoveData
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("afterDays")]
        public double? AfterDays { get; set; }
    }

    public partial class JobScripts
    {
        [JsonProperty("periodicityType")]
        public string? PeriodicityType { get; set; }

        [JsonProperty("preCommand")]
        public PreCommand? PreCommand { get; set; }

        [JsonProperty("postCommand")]
        public PostCommand? PostCommand { get; set; }

        [JsonProperty("runScriptEvery")]
        public double? RunScriptEvery { get; set; }

        [JsonProperty("dayOfWeek")]
        public List<string>? DayOfWeek { get; set; }
    }

    public partial class PostCommand
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("command")]
        public string? Command { get; set; }
    }

    public partial class PreCommand
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("command")]
        public string? Command { get; set; }
    }

    public partial class GuestProcessing
    {
        [JsonProperty("appAwareProcessing")]
        public AppAwareProcessing AppAwareProcessing { get; set; }

        [JsonProperty("guestFSIndexing")]
        public GuestFsIndexing GuestFsIndexing { get; set; }

        [JsonProperty("guestInteractionProxies")]
        public GuestInteractionProxies? GuestInteractionProxies { get; set; }

        [JsonProperty("guestCredentials")]
        public GuestCredentials? GuestCredentials { get; set; }
    }

    public partial class AppAwareProcessing
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("appSettings")]
        public List<AppSetting>? AppSettings { get; set; }
    }

    public partial class AppSetting
    {
        [JsonProperty("vmObject")]
        public InventoryObject VmObject { get; set; }

        [JsonProperty("vss")]
        public string Vss { get; set; }

        [JsonProperty("usePersistentGuestAgent")]
        public bool? UsePersistentGuestAgent { get; set; }

        [JsonProperty("transactionsLogs")]
        public string? TransactionsLogs { get; set; }

        [JsonProperty("sql")]
        public SQL? SQL { get; set; }

        [JsonProperty("oracle")]
        public Oracle? Oracle { get; set; }

        [JsonProperty("exclusions")]
        public Exclusions? Exclusions { get; set; }

        [JsonProperty("scripts")]
        public Scripts? Scripts { get; set; }
    }

    public partial class Exclusions
    {
        [JsonProperty("exclusionPolicy")]
        public string ExclusionPolicy { get; set; }

        [JsonProperty("itemsList")]
        public List<string>? ItemsList { get; set; }
    }

    public partial class GuestCredentials
    {
        [JsonProperty("credsType")]
        public string CredsType { get; set; }

        [JsonProperty("credsId")]
        public string CredsId { get; set; }

        [JsonProperty("credentialsPerMachine")]
        public List<CredentialsPerMachine>? CredentialsPerMachine { get; set; }
    }

    public partial class CredentialsPerMachine
    {
        [JsonProperty("windowsCredsId")]
        public Guid? WindowsCredsId { get; set; }

        [JsonProperty("linuxCredsId")]
        public Guid? LinuxCredsId { get; set; }

        [JsonProperty("objectId")]
        public InventoryObject ObjectId { get; set; }
    }

    public partial class Oracle
    {
        [JsonProperty("useGuestCredentials")]
        public bool UseGuestCredentials { get; set; }

        [JsonProperty("credentialsId")]
        public Guid? CredentialsId { get; set; }

        [JsonProperty("archiveLogs")]
        public string ArchiveLogs { get; set; }

        [JsonProperty("deleteHoursCount")]
        public double? DeleteHoursCount { get; set; }

        [JsonProperty("deleteGBsCount")]
        public double? DeleteGBsCount { get; set; }

        [JsonProperty("backupLogs")]
        public bool? BackupLogs { get; set; }

        [JsonProperty("backupMinsCount")]
        public double? BackupMinsCount { get; set; }

        [JsonProperty("retainLogBackups")]
        public string? RetainLogBackups { get; set; }

        [JsonProperty("keepDaysCount")]
        public double? KeepDaysCount { get; set; }

        [JsonProperty("logShippingServers")]
        public LogShippingServers? LogShippingServers { get; set; }
    }

    public partial class LogShippingServers
    {
        [JsonProperty("autoSelection")]
        public bool AutoSelection { get; set; }

        [JsonProperty("shippingServerIds")]
        public List<Guid>? ShippingServerIds { get; set; }
    }

    public partial class Scripts
    {
        [JsonProperty("scriptProcessingMode")]
        public string ScriptProcessingMode { get; set; }

        [JsonProperty("windowsScripts")]
        public WindowsScripts? WindowsScripts { get; set; }

        [JsonProperty("linuxScripts")]
        public LinuxScripts? LinuxScripts { get; set; }
    }

    public partial class LinuxScripts
    {
        [JsonProperty("preFreezeScript")]
        public string? PreFreezeScript { get; set; }

        [JsonProperty("postThawScript")]
        public string? PostThawScript { get; set; }
    }

    public partial class SQL
    {
        [JsonProperty("logsProcessing")]
        public string LogsProcessing { get; set; }

        [JsonProperty("backupMinsCount")]
        public double? BackupMinsCount { get; set; }

        [JsonProperty("retainLogBackups")]
        public string? RetainLogBackups { get; set; }

        [JsonProperty("keepDaysCount")]
        public double? KeepDaysCount { get; set; }

        [JsonProperty("logShippingServers")]
        public LogShippingServers? LogShippingServers { get; set; }
    }

    public partial class WindowsScripts
    {
        [JsonProperty("preFreezeScript")]
        public string? PreFreezeScript { get; set; }

        [JsonProperty("postThawScript")]
        public string? PostThawScript { get; set; }
    }

    public partial class GuestFsIndexing
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("indexingSettings")]
        public List<IndexingSetting> IndexingSettings { get; set; }
    }

    public partial class IndexingSetting
    {
        [JsonProperty("vmObject")]
        public InventoryObject VmObject { get; set; }

        [JsonProperty("WindowsIndexing")]
        public WindowsIndexing? WindowsIndexing { get; set; }

        [JsonProperty("LinuxIndexing")]
        public LinuxIndexing? LinuxIndexing { get; set; }
    }

    public partial class LinuxIndexing
    {
        [JsonProperty("guestFSIndexingMode")]
        public string GuestFsIndexingMode { get; set; }

        [JsonProperty("indexingList")]
        public List<string>? IndexingList { get; set; }
    }

    public partial class WindowsIndexing
    {
        [JsonProperty("guestFSIndexingMode")]
        public string GuestFsIndexingMode { get; set; }

        [JsonProperty("indexingList")]
        public List<string>? IndexingList { get; set; }
    }

    public partial class GuestInteractionProxies
    {
        [JsonProperty("autoSelection")]
        public bool AutoSelection { get; set; }

        [JsonProperty("proxyIds")]
        public List<Guid>? ProxyIds { get; set; }
    }

    public partial class Schedule
    {
        [JsonProperty("runAutomatically")]
        public bool RunAutomatically { get; set; }

        [JsonProperty("daily")]
        public Daily? Daily { get; set; }

        [JsonProperty("monthly")]
        public Monthly? Monthly { get; set; }

        [JsonProperty("periodically")]
        public Periodically? Periodically { get; set; }

        [JsonProperty("continuously")]
        public Continuously? Continuously { get; set; }

        [JsonProperty("afterThisJob")]
        public AfterThisJob? AfterThisJob { get; set; }

        [JsonProperty("retry")]
        public Retry? Retry { get; set; }

        [JsonProperty("backupWindow")]
        public BackupWindow? BackupWindow { get; set; }
    }

    public partial class AfterThisJob
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("jobName")]
        public string? JobName { get; set; }
    }

    public partial class Continuously
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("backupWindow")]
        public WindowSetting BackupWindow { get; set; }
    }

    public partial class Daily
    {
        [JsonProperty("dailyKind")]
        public string? DailyKind { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("localTime")]
        public DateTime? LocalTime { get; set; }

        [JsonProperty("days")]
        public List<string>? Days { get; set; }
    }

    public partial class Periodically
    {
        [JsonProperty("periodicallyKind")]
        public string? PeriodicallyKind { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("frequency")]
        public double? Frequency { get; set; }

        [JsonProperty("backupWindow")]
        public WindowSetting BackupWindow { get; set; }

        [JsonProperty("startTimeWithinAnHour")]
        public double? StartTimeWithinAnHour { get; set; }
    }

    public partial class Retry
    {
        [JsonProperty("isEnabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("retryCount")]
        public double? RetryCount { get; set; }

        [JsonProperty("awaitMinutes")]
        public double? AwaitMinutes { get; set; }
    }

    public partial class BackupWindow
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("backupWindow")]
        public WindowSetting WindowSetting { get; set; }
    }

    public partial class WindowSetting
    {
        [JsonProperty("backupWindow")]
        public List<Day>? Days { get; set; }
    }

    public partial class Day
    {
        [JsonProperty("day")]
        public string DayOfWeek { get; set; }

        [JsonProperty("hours")]
        public string Hours { get; set; }
    }

    // Job State
    public partial class States
    {
        [JsonProperty("data")]
        public List<State> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public partial class State
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lastResult")]
        public string LastResult { get; set; }

        [JsonProperty("workload")]
        public string Workload { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lastRun")]
        public DateTimeOffset? LastRun { get; set; }

        [JsonProperty("nextRun")]
        public DateTimeOffset? NextRun { get; set; }

        [JsonProperty("repositoryId")]
        public Guid? RepositoryId { get; set; }

        [JsonProperty("repositoryName")]
        public string? RepositoryName { get; set; }

        [JsonProperty("objectsCount")]
        public double ObjectsCount { get; set; }

        [JsonProperty("sessionId")]
        public Guid? SessionId { get; set; }
    }
}
