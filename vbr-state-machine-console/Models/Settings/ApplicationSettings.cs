using Newtonsoft.Json;
using System;

namespace vbr_state_machine_console.Models.Settings
{



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

        [JsonProperty("TeamsAlwaysNotifyStatus")]
        public bool TeamsAlwaysNotifyStatus { get; set; }

        [JsonProperty("TeamsWebHookUri")]
        public Uri TeamsWebHookUri { get; set; }

        [JsonProperty("xMattersEnabled")]
        public bool XMattersEnabled { get; set; }

        [JsonProperty("xMattersTriggerUrl")]
        public string XMattersTriggerUrl { get; set; }

        [JsonProperty("xMattersHostname")]
        public string XMattersHostname { get; set; }

        [JsonProperty("xMattersRecipient")]
        public string XMattersRecipient { get; set; }

        [JsonProperty("xMattersTriggerUsername")]
        public string XMattersTriggerUsername { get; set; }

        [JsonProperty("xMattersTriggerPassword")]
        public string XMattersTriggerPassword { get; set; }

        [JsonProperty("SantaOpsHostname")]
        public string SantaOpsHostname { get; set; }

        [JsonProperty("SantaOpsEndpoint")]
        public string SantaOpsEndpoint { get; set; }

    }
}
