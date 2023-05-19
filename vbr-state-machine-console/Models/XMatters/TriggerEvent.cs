using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.XMatters
{    

 public partial class TriggerEvent
    {
        [JsonProperty("recipients")]
        public string Recipients { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }
    }

}