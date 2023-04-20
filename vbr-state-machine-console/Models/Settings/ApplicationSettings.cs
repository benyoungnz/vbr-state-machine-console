using Newtonsoft.Json;
using System;

namespace vbr_state_machine_console.Models.Settings
{



    public class DesiredStates
    {
        [JsonProperty("CapacityTier")]
        public DesiredState.CapacityTier CapacityTier { get; set; }

        [JsonProperty("Users")]
        public DesiredState.User Users { get; set; }
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
