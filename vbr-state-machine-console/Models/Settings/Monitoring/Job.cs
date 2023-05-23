using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.Monitoring
{
    public class Job
    {
        [JsonProperty("RpoDays")]
        public double RpoDays { get; set; }
    }
}
