using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using vbr_state_machine_console.Models.VBR;

namespace vbr_state_machine_console.Models.VBR.RepoState
{
    public partial class RepositoryStates
    {
        [JsonProperty("data")]
        public List<RepositoryState> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public partial class RepositoryState
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("hostId")]
        public Guid? HostId { get; set; }

        [JsonProperty("hostName")]
        public string HostName { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("capacityGB")]
        public long CapacityGb { get; set; }

        [JsonProperty("freeGB")]
        public double FreeGb { get; set; }

        [JsonProperty("usedSpaceGB")]
        public double UsedSpaceGb { get; set; }
    }
}
