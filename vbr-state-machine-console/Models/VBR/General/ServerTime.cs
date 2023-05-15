using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbr_state_machine_console.Models.VBR.General
{
    public partial class ServerTime
    {
        [JsonProperty("serverTime")]
        public string CurrentTime { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
    }
}
