using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbr_state_machine_console.Models.Settings
{
    public class BackupServer
    {
        [JsonProperty("Host")]
        public string Host { get; set; }

        [JsonProperty("Port")]
        public long Port { get; set; }

        [JsonProperty("APIVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("APIRouteVersion")]
        public string ApiRouteVersion { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
