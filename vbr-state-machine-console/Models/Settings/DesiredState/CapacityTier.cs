using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{
    public class CapacityTier
    {
        [JsonProperty("Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("CapacityTier")]
        public bool EnforceEncryption { get; set; }

        [JsonProperty("FreeSpaceCriticalPercent")]
        public bool ImmediateCopyRequired { get; set; }
    }
}
