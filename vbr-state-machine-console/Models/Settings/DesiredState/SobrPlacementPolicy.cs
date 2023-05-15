using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{
    public class SobrPlacementPolicy
    {
        [JsonProperty("PlacementType")]
        public string PlacementType { get; set; }


    }
}
