using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using vbr_state_machine_console.Models.VBR;

namespace vbr_state_machine_console.Models.VBR.SOBR
{
    public partial class ScaleoutRepositories
    {
        [JsonProperty("data")]
        public List<ScaleoutRepository> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
    public partial class ScaleoutRepository
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("performanceTier")]
        public PerformanceTier PerformanceTier { get; set; }

        [JsonProperty("placementPolicy")]
        public PlacementPolicy PlacementPolicy { get; set; }

        [JsonProperty("capacityTier")]
        public CapacityTier CapacityTier { get; set; }

        [JsonProperty("archiveTier")]
        public ArchiveTier ArchiveTier { get; set; }
    }

    public partial class ArchiveTier
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("extentId")]
        public object ExtentId { get; set; }

        [JsonProperty("archivePeriodDays")]
        public long ArchivePeriodDays { get; set; }

        [JsonProperty("advancedSettings")]
        public ArchiveTierAdvancedSettings AdvancedSettings { get; set; }
    }

    public partial class ArchiveTierAdvancedSettings
    {
        [JsonProperty("costOptimizedArchiveEnabled")]
        public bool CostOptimizedArchiveEnabled { get; set; }

        [JsonProperty("archiveDeduplicationEnabled")]
        public bool ArchiveDeduplicationEnabled { get; set; }
    }

    public partial class CapacityTier
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("extentId")]
        public Guid ExtentId { get; set; }

        [JsonProperty("offloadWindow")]
        public OffloadWindow OffloadWindow { get; set; }

        [JsonProperty("copyPolicyEnabled")]
        public bool CopyPolicyEnabled { get; set; }

        [JsonProperty("movePolicyEnabled")]
        public bool MovePolicyEnabled { get; set; }

        [JsonProperty("operationalRestorePeriodDays")]
        public long OperationalRestorePeriodDays { get; set; }

        [JsonProperty("overridePolicy")]
        public OverridePolicy OverridePolicy { get; set; }

        [JsonProperty("encryption")]
        public Encryption Encryption { get; set; }
    }

    public partial class Encryption
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("encryptionPasswordIdOrNull")]
        public Guid EncryptionPasswordIdOrNull { get; set; }

        [JsonProperty("encryptionPasswordTag")]
        public object EncryptionPasswordTag { get; set; }
    }

    public partial class OffloadWindow
    {
        [JsonProperty("days")]
        public List<Day> Days { get; set; }
    }

    public partial class Day
    {
        [JsonProperty("day")]
        public string DayDay { get; set; }

        [JsonProperty("hours")]
        public string Hours { get; set; }
    }

    public partial class OverridePolicy
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("overrideSpaceThresholdPercents")]
        public long OverrideSpaceThresholdPercents { get; set; }
    }

    public partial class PerformanceTier
    {
        [JsonProperty("performanceExtents")]
        public List<PerformanceExtent> PerformanceExtents { get; set; }

        [JsonProperty("advancedSettings")]
        public PerformanceTierAdvancedSettings AdvancedSettings { get; set; }
    }

    public partial class PerformanceTierAdvancedSettings
    {
        [JsonProperty("perVmBackup")]
        public bool PerVmBackup { get; set; }

        [JsonProperty("fullWhenExtentOffline")]
        public bool FullWhenExtentOffline { get; set; }
    }

    public partial class PerformanceExtent
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class PlacementPolicy
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("settings")]
        public List<Setting> Settings { get; set; }
    }

    public partial class Setting
    {
        [JsonProperty("allowedBackups")]
        public string AllowedBackups { get; set; }

        [JsonProperty("extentName")]
        public object ExtentName { get; set; }
    }
}
