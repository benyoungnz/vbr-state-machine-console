using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.VBR
{
    public partial class Pagination
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("skip")]
        public long Skip { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }
    }
}

