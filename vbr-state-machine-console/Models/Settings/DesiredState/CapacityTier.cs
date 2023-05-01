using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{
    public class SobrCapacityTier
    {
        [JsonProperty("Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("EnforceEncryption")]
        public bool EnforceEncryption { get; set; }

        [JsonProperty("FreeSpaceCriticalPercent")]
        public bool ImmediateCopyRequired { get; set; }

        [JsonProperty("MoveEnabled")]
        public bool MoveEnabled { get; set; }

        [JsonProperty("MoveAfterDays")]
        public int MoveAfterDays { get; set; }
    }
}
