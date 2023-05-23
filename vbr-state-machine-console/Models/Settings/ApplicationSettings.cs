using Newtonsoft.Json;
using System;

namespace vbr_state_machine_console.Models.Settings
{


    public class Monitors
    {
        [JsonProperty("SobrCapacityTier")]
        public Monitoring.SobrCapacityTier SobrCapacityTier { get; set; }

        [JsonProperty("SobrPerformanceTier")]
        public Monitoring.SobrPerformanceTier SobrPerformanceTier { get; set; }
    }
    public class DesiredStates
    {
        [JsonProperty("JobConfig")]
        public DesiredState.JobConfig JobConfig { get; set; }
        
        [JsonProperty("SobrPerformanceTier")]
        public DesiredState.SobrPerformanceTier SobrPerformanceTier { get; set; }

        [JsonProperty("SobrPlacementPolicy")]
        public DesiredState.SobrPlacementPolicy SobrPlacementPolicy { get; set; }

        [JsonProperty("SobrCapacityTier")]
        public DesiredState.SobrCapacityTier SobrCapacityTier { get; set; }

        [JsonProperty("SobrArchiveTier")]
        public DesiredState.SobrArchiveTier SobrArchiveTier { get; set; }

        [JsonProperty("GeneralOptions")]
        public DesiredState.GeneralOptions GeneralOptions { get; set; }
    }


    public class Alerts
    {

        [JsonProperty("TeamsEnabled")]
        public bool TeamsEnabled { get; set; }
        [JsonProperty("SantaOpsEnabled")]
        public bool SantaOpsEnabled { get; set; }

        [JsonProperty("TeamsWebHookUri")]
        public Uri TeamsWebHookUri { get; set; }

        [JsonProperty("xMattersEnabled")]
        public bool XMattersEnabled { get; set; }

        [JsonProperty("xMattersHookPath")]
        public string xMattersHookPath { get; set; }
        
        [JsonProperty("xMattersBase")]
        public string xMattersBase { get; set; }

        [JsonProperty("xMattersRecipients")]
        public string xMattersRecipients { get; set; }

        [JsonProperty("SantaOpsUri")]
        public string SantaOpsUri { get; set; }

    }
}
