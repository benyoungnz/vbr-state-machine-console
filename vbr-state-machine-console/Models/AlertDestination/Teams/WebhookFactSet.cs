using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace vbr_state_machine_console.Models.AlertDestination.Teams
{
    public partial class Webhook
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "message";

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }
    }

    public partial class Attachment
    {
        [JsonProperty("contentType")]
        public string Type { get; set; } = "application/vnd.microsoft.card.adaptive";

        [JsonProperty("$schema")]
        public string Schema { get; set; } = "http://adaptivecards.io/schemas/adaptive-card.json";

        [JsonProperty("version")]
        public string Version { get; set; } = "1.5";

        [JsonProperty("content")]
        public Content Content { get; set; }
    }

    //public partial class AdaptiveCard
    //{
    //    //[JsonProperty("type")]
    //    //public string Type { get; set; } = "AdaptiveCard";

    //    [JsonProperty("contentType")]
    //    public string ContentType { get; set; } = "application/vnd.microsoft.card.adaptive";

    //    [JsonProperty("body")]
    //    public List<dynamic> Body { get; set; }

    //    [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
    //    public List<Action> Actions { get; set; }

    //    [JsonProperty("$schema")]
    //    public string Schema { get; set; } = "http://adaptivecards.io/schemas/adaptive-card.json";

    //    [JsonProperty("version")]
    //    public string Version { get; set; } = "1.4";
    //}

    public partial class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "AdaptiveCard";

        [JsonProperty("body")]
        public List<dynamic> Body { get; set; }

        [JsonProperty("actions")]
        public List<Action> Actions { get; set; }
    }

    public partial class Action
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class BodyTextBlock
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "TextBlock";

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; } = "default";

        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public string Weight { get; set; } = "default";

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("wrap", NullValueHandling = NullValueHandling.Ignore)]
        public bool Wrap { get; set; } = true;

        [JsonProperty("isSubtle", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSubtle { get; set; } = true;

        [JsonProperty("color")]
        public string Color { get; set; } = "default";

    }

    public partial class BodyFactSet
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "FactSet";

        [JsonProperty("facts", NullValueHandling = NullValueHandling.Ignore)]
        public List<Fact> Facts { get; set; }
    }

    public partial class Fact
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

    }
}
