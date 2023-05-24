using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.Monitoring
{
    public class SobrPerformanceTier
    {
        [JsonProperty("PercentFreeWarning")]
        public int PercentFreeWarning { get; set; }

        [JsonProperty("PercentFreeCritical")]
        public int PercentFreeCritical { get; set; }
    }
}
