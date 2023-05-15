using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbr_state_machine_console.Models.VBR.General
{
    public partial class GeneralOptions
    {
        [JsonProperty("emailSettings")]
        public EmailSettings EmailSettings { get; set; }

        [JsonProperty("notifications")]
        public Notifications Notifications { get; set; }
    }

    public class EmailSettings
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("smtpServerName")]
        public string SmtpServerName { get; set; }

        [JsonProperty("advancedSmtpOptions")]
        public AdvancedSmtpOptions AdvancedSmtpOptions { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("sendDailyReportsAt")]
        public string SendDailyReportsAt { get; set; }

        [JsonProperty("notifyOnSuccess")]
        public bool NotifyOnSuccess { get; set; }

        [JsonProperty("notifyOnWarning")]
        public bool NotifyOnWarning { get; set; }

        [JsonProperty("notifyOnFailure")]
        public bool NotifyOnFailure { get; set; }

        [JsonProperty("notifyOnLastRetry")]
        public bool NotifyOnLastRetry { get; set; }
    }

    public class AdvancedSmtpOptions
    {
        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("timeoutMs")]
        public long TimeoutMs { get; set; }

        [JsonProperty("SSLEnabled")]
        public bool SslEnabled { get; set; }

        [JsonProperty("authRequred")]
        public bool AuthRequred { get; set; }

        [JsonProperty("credentialsId")]
        public object CredentialsId { get; set; }
    }

    public class Notifications
    {
        [JsonProperty("storageSpaceThresholdEnabled")]
        public bool StorageSpaceThresholdEnabled { get; set; }

        [JsonProperty("storageSpaceThreshold")]
        public long StorageSpaceThreshold { get; set; }

        [JsonProperty("datastoreSpaceThresholdEnabled")]
        public bool DatastoreSpaceThresholdEnabled { get; set; }

        [JsonProperty("datastoreSpaceThreshold")]
        public long DatastoreSpaceThreshold { get; set; }

        [JsonProperty("skipVMSpaceThresholdEnabled")]
        public bool SkipVmSpaceThresholdEnabled { get; set; }

        [JsonProperty("skipVMSpaceThreshold")]
        public long SkipVmSpaceThreshold { get; set; }

        [JsonProperty("notifyOnSupportExpiration")]
        public bool NotifyOnSupportExpiration { get; set; }

        [JsonProperty("notifyOnUpdates")]
        public bool NotifyOnUpdates { get; set; }
    }
}
