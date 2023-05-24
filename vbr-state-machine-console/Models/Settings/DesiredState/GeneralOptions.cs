using Newtonsoft.Json;

namespace vbr_state_machine_console.Models.Settings.DesiredState
{
    public partial class GeneralOptions
    {

        [JsonProperty("autoRemdediate")]
        public bool AutoRemdediate { get; set; }
        
        [JsonProperty("EmailIsEnabled")]
        public bool EmailIsEnabled { get; set; }

        [JsonProperty("EmailTo")]
        public string EmailTo { get; set; }

        [JsonProperty("EmailFrom")]
        public string EmailFrom { get; set; }

        [JsonProperty("EmailSmtpServer")]
        public string EmailSmtpServer { get; set; }

        [JsonProperty("EmailAdvPort")]
        public long EmailAdvPort { get; set; }

        [JsonProperty("EmailSSLEnabled")]
        public bool EmailSslEnabled { get; set; }

        [JsonProperty("NotifyOnSuccess")]
        public bool NotifyOnSuccess { get; set; }

        [JsonProperty("NotifyOnWarning")]
        public bool NotifyOnWarning { get; set; }

        [JsonProperty("NotifyOnFailure")]
        public bool NotifyOnFailure { get; set; }

        [JsonProperty("NotifyOnUpdates")]
        public bool NotifyOnUpdates { get; set; }

        [JsonProperty("NotifyOnSupportExpiration")]
        public bool NotifyOnSupportExpiration { get; set; }

        [JsonProperty("ServerTimeZone")]
        public string ServerTimeZone { get; set; }
    }
}