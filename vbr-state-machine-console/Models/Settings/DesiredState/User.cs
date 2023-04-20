using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{
    public class User
    {
        [JsonProperty("EnforceMFA")]
        public bool EnforceMFA { get; set; }

    }
}