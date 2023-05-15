using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{
    public class SobrPerformanceTier
    {
        [JsonProperty("PerVmBackup")]
        public bool PerVmBackup { get; set; }


    }
}
