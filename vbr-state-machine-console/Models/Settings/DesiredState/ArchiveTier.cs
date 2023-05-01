using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{
    public class SobrArchiveTier
    {
        [JsonProperty("Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("CostOptimizedEnabled")]
        public bool CostOptimizedEnabled { get; set; }

        [JsonProperty("DedupeEnabled")]
        public bool DedupeEnabled { get; set; }
        
        [JsonProperty("ArchivePeriodDays")]
        public int ArchivePeriodDays { get; set; }

    }
}
