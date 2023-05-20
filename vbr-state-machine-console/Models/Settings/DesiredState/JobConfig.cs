using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{

    public partial class JobConfig
    {
        [JsonProperty("IsDisabled")]
        public bool IsDisabled { get; set; }
        
        [JsonProperty("Storage")]
        public Storage Storage { get; set; }

        [JsonProperty("Exclusions")]
        public string[]? Exclusions { get; set; }
    }

    public partial class Storage
    {
        [JsonProperty("RetentionPolicy")]
        public RetentionPolicy RetentionPolicy { get; set; }

        [JsonProperty("AdvancedSettings")]
        public AdvancedSettings? AdvancedSettings { get; set; }
    }

    public partial class RetentionPolicy
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Quantity")]
        public double Quantity { get; set; }
    }

    public partial class AdvancedSettings
    {
        [JsonProperty("BackupModeType")]
        public string BackupModeType { get; set; }

        [JsonProperty("StorageData")]
        public StorageData StorageData { get; set; }

        [JsonProperty("Notifications")]
        public Notifications Notifications { get; set; }

        [JsonProperty("VSphere")]
        public VSphere VSphere { get; set; }

        [JsonProperty("StorageIntegration")]
        public StorageIntegration StorageIntegration { get; set; }
    }

    public partial class Notifications
    {
        [JsonProperty("SendSnmpNotifications")]
        public bool SendSnmpNotifications { get; set; }

        [JsonProperty("EmailNotifications")]
        public EmailNotifications EmailNotifications { get; set; }
    }

    public partial class EmailNotifications
    {
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
    }

    public partial class VSphere
    {
        [JsonProperty("EnableVmwareToolsQuiescence")]
        public bool EnableVmwareToolsQuiescence { get; set; }

        [JsonProperty("ChangedBlockTracking")]
        public ChangedBlockTracking ChangedBlockTracking { get; set; }
    }

    public partial class ChangedBlockTracking
    {
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("EnableCbtautomatically")]
        public bool EnableCbtautomatically { get; set; }

        [JsonProperty("ResetCbtonActiveFull")]
        public bool ResetCbtonActiveFull { get; set; }
    }

    public partial class StorageData
    {
        [JsonProperty("CompressionLevel")]
        public string CompressionLevel { get; set; }

        [JsonProperty("StorageOptimization")]
        public string StorageOptimization { get; set; }

        [JsonProperty("EnableInlineDataDedup")]
        public bool EnableInlineDataDedup { get; set; }

        [JsonProperty("Encryption")]
        public Encryption Encryption { get; set; }
    }

    public partial class Encryption
    {
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
    }

    public partial class StorageIntegration
    {
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
    }
}