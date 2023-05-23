using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.SantaOps
{    

 public partial class TriggerEvent
    {
        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("triggeredOn")]
        public string TriggeredOn { get; set; }
    }

}