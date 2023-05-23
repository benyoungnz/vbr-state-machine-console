using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using vbr_state_machine_console.Models.AlertDestination;
using vbr_state_machine_console.Models.Settings;
using vbr_state_machine_console.Models.VBR;
using veeam_repository_reporter;

namespace vbr_state_machine_console.Integration
{
    internal class Teams
    {

        private Models.Settings.Alerts alertSettings {get;set;}
        private string emojiOk = "✅";
        private string emojiWarning = "📣";
        private string emojiCritical = "🚨";

        public Teams(Models.Settings.Alerts alertSettings) { 
     
            this.alertSettings = alertSettings;
        }

        public void TriggerAlerts(List<GenericAlert> lstAlerts, string bkpServerHostname)
        {

             //group by
            var teamsAlertAttachments = new List<dynamic>(); //the alert for THIS sobr
            var teamsFactSet = new Models.AlertDestination.Teams.BodyFactSet() { Facts = new List<Models.AlertDestination.Teams.Fact>() };
            var lastParent = "";
            foreach (var sc in lstAlerts)
            {
                if (lastParent != sc.Parent) //separator {
                {
                    teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"-", Value = $"" });
                    teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"{sc.Parent}", Value = $"" });
                    teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"-", Value = $"" });

                }

                var emj = sc.Priority == "CRITICAL" ? emojiCritical : emojiWarning;
                teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"{sc.Property}", Value = $"{emj} {sc.Subject}, {sc.Message}" });
                lastParent = sc.Parent;
               
            }

            teamsAlertAttachments.Add(new Models.AlertDestination.Teams.BodyTextBlock() { Text = $"Monitors", Size = "large" });
            teamsAlertAttachments.Add(new Models.AlertDestination.Teams.BodyTextBlock() { Text = $"Captured {DateTime.Now} for host **{bkpServerHostname}**" });

            teamsAlertAttachments.Add(teamsFactSet); //facts 
            TriggerWebhook(teamsAlertAttachments, "Monitors"); //send teams alert.

   
        }


        public void TriggerDesiredState(List<GenericCompare> lstStateTracking, string bkpServerHostname)
        {

             //group by
            var teamsAlertAttachments = new List<dynamic>(); //the alert for THIS sobr
            var teamsFactSet = new Models.AlertDestination.Teams.BodyFactSet() { Facts = new List<Models.AlertDestination.Teams.Fact>() };
            var lastParent = "";
            foreach (var sc in lstStateTracking)
            {
                if (lastParent != sc.Parent) //separator {
                {
                    teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"-", Value = $"" });
                    teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"{sc.Parent}", Value = $"" });
                    teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"-", Value = $"" });

                }

                var emj = sc.AlertRequired ? emojiCritical : emojiOk;
                teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"{sc.Property}", Value = $"{emj} {sc.Description}" });
                lastParent = sc.Parent;
               
            }

            teamsAlertAttachments.Add(new Models.AlertDestination.Teams.BodyTextBlock() { Text = $"Desired State Comparer", Size = "large" });
            teamsAlertAttachments.Add(new Models.AlertDestination.Teams.BodyTextBlock() { Text = $"Captured {DateTime.Now} for host **{bkpServerHostname}**" });

            teamsAlertAttachments.Add(teamsFactSet); //facts 
            TriggerWebhook(teamsAlertAttachments, "State Summary"); //send teams alert.

   
        }

        private void TriggerWebhook(List<dynamic> content, string description)
        {

            var teamsMessage = new Models.AlertDestination.Teams.Webhook()
            {
                Attachments = new List<Models.AlertDestination.Teams.Attachment>()
            };

            var mainAttachment = new Models.AlertDestination.Teams.Attachment() { Content = new Models.AlertDestination.Teams.Content() { Body = new List<dynamic>() } };

            foreach (var c in content)
            {
                mainAttachment.Content.Body.Add(c);
            }

            teamsMessage.Attachments.Add(mainAttachment);



            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(teamsMessage);
                try
                {
                    var result = client.PostAsync(alertSettings.TeamsWebHookUri, new StringContent(json, null, "application/json")).Result;
                    var x = result;
                    ColorConsole.WriteWrappedHeader($"Teams {description} webhook sent", headerColor: ConsoleColor.Green);

                }
                catch (Exception ex)
                {

                    ColorConsole.WriteWrappedHeader($"Error sending {description} teams webhook", headerColor: ConsoleColor.Red);
                    ColorConsole.WriteError(ex.Message);

                }

            }

        }

    }
}
