using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbr_state_machine_console.Models.AlertDestination.XMatters
{
    public partial class TriggerEvent
    {
        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("recipients")]
        public List<Recipient> Recipients { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("Summary")]
        public string Summary { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }

    public partial class Recipient
    {
        [JsonProperty("targetName")]
        public string TargetName { get; set; }
    }
}
