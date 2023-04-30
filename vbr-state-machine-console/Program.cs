using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using vbr_state_machine_console.Models.Settings;
using vbr_state_machine_console.Models.VBR;
using veeam_repository_reporter;

namespace vbr_state_machine_console
{


    internal class Program
    {
        private static Models.Settings.Alerts settingsAlerts { get; set; }
        private static Models.Settings.DesiredStates settingsDesiredStates { get; set; }

        static void Main(string[] args)
        {

            //setup config
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

            //alert settings
            settingsAlerts = config.GetRequiredSection("Alerts").Get<Models.Settings.Alerts>();

            //desired states settings
            settingsDesiredStates = config.GetRequiredSection("DesiredStates").Get<Models.Settings.DesiredStates>();

            //monitoring
            //TBC
            //settingsDesiredStates = config.GetRequiredSection("DesiredStates").Get<Models.Settings.Monitoring>(); 

            var backupServers = config.GetRequiredSection("BackupServers").Get<List<Models.Settings.BackupServer>>();

            foreach (var backupServer in backupServers)
            {
                ProcessBackupServer(backupServer);
            }



        }

        private static Models.AlertDestination.GenericCompare StateComparer(object expected, object actual, string Property, string Parent, BackupServer bkpServer)
        {
            var gc = new Models.AlertDestination.GenericCompare()
            {
                Property = Property,
                Parent = Parent,
                VBRServer = bkpServer.Host,
                Actual = actual.ToString(),
                Expected = expected.ToString(),
                AlertRequired = !expected.Equals(actual),
            };

            if (!gc.AlertRequired)
            {
                gc.Description = $"Configured as expected ({gc.Expected})";
                ColorConsole.WriteEmbeddedColorLine($"{Property}: [green]{gc.Description}[/green]");
            }

            else
            {
                gc.Description = $"Expected: {gc.Expected}, Actual: {gc.Actual}";
                ColorConsole.WriteEmbeddedColorLine($"{Property}: [yellow]{gc.Description}[/yellow]");
            }



            return gc;

        }

        private static void ProcessBackupServer(BackupServer bkpServer)
        {
            //fire up a VBR session
            var vbrSession = new Integration.VBR(bkpServer);


            //globals for alerts and keeping track of state 
            var lstStateTracking = new List<Models.AlertDestination.GenericCompare>();

            
            //sobr
            foreach (var sobr in vbrSession.GetSOBRS())
            {
            



                ColorConsole.WriteWrappedHeader($"{sobr.Name}", headerColor: ConsoleColor.Green);

                ColorConsole.WriteEmbeddedColorLine($"Capacity Tier: [green]{sobr.CapacityTier.Enabled}[/green] // Archive Tier: [green]{sobr.ArchiveTier.IsEnabled}[/green]");
                ColorConsole.WriteEmbeddedColorLine($"Placement: [yellow]{sobr.PlacementPolicy.Type}[/yellow] // ID: [yellow]{sobr.Id}[/yellow]");

                //desired state checks

                //capacity tier enabled check
                lstStateTracking.Add(StateComparer(settingsDesiredStates.CapacityTier.Enabled, sobr.CapacityTier.Enabled, "Capacity Tier Enabled", sobr.Name, bkpServer));
              
            }


            //job states
            foreach (var jobState in vbrSession.GetJobStates())
            {


                ColorConsole.WriteWrappedHeader($"{jobState.Name}", headerColor: ConsoleColor.Green);

                ColorConsole.WriteEmbeddedColorLine($"Status: [green]{jobState.Status}[/green]");
                ColorConsole.WriteEmbeddedColorLine($"Last Run: [green]{jobState.LastRun}[/green]");

                //desired state checks

                //capacity tier enabled check
                // lstStateTracking.Add(StateComparer(settingsDesiredStates.CapacityTier.Enabled, sobr.CapacityTier.Enabled, "Capacity Tier Enabled", sobr.Name, bkpServer));
              
            }

            // //job configs
            foreach (var jobConfig in vbrSession.GetJobConfigs())
            {


                ColorConsole.WriteWrappedHeader($"{jobConfig.Name}", headerColor: ConsoleColor.Green);

                ColorConsole.WriteEmbeddedColorLine($"Description: [green]{jobConfig.Description}[/green]");
                ColorConsole.WriteEmbeddedColorLine($"Disabled: [green]{jobConfig.IsDisabled}[/green]");

                //desired state checks

                //capacity tier enabled check
                // lstStateTracking.Add(StateComparer(settingsDesiredStates.CapacityTier.Enabled, sobr.CapacityTier.Enabled, "Capacity Tier Enabled", sobr.Name, bkpServer));
              
            }

            var emojiOk = "✅";
            var emojiWarning = "📣";
            var emojiCritical = "🚨";

            //process alerts, for now dump everything, need to use the alwaysAlert teams etc to push potentially just violations of rules
            //TODO: move to helper function and provide more functionality and formatting, just a quick test example
            var teamsAlertAttachments = new List<dynamic>(); //the alert for THIS sobr
            var teamsFactSet = new Models.AlertDestination.Teams.BodyFactSet() { Facts = new List<Models.AlertDestination.Teams.Fact>() };
            foreach (var sc in lstStateTracking)
            {
                var emj = sc.AlertRequired ? emojiCritical : emojiOk;
                teamsFactSet.Facts.Add(new Models.AlertDestination.Teams.Fact() { Title = $"{sc.Property} {emj}", Value = $"{sc.Description} on {sc.Parent}" });
               
            }

            teamsAlertAttachments.Add(new Models.AlertDestination.Teams.BodyTextBlock() { Text = $"Desired State Comparer", Size = "large" });
            teamsAlertAttachments.Add(new Models.AlertDestination.Teams.BodyTextBlock() { Text = $"Captured {DateTime.Now} for host **{bkpServer.Host}**" });

            teamsAlertAttachments.Add(teamsFactSet); //facts 
            SendTeamsWebhook(teamsAlertAttachments); //send teams alert.


        }

        public static void SendTeamsWebhook(List<dynamic> content)
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
                    var result = client.PostAsync(settingsAlerts.TeamsWebHookUri, new StringContent(json, null, "application/json")).Result;
                    var x = result;
                    ColorConsole.WriteWrappedHeader($"Webhook sent", headerColor: ConsoleColor.Green);

                }
                catch (Exception ex)
                {

                    ColorConsole.WriteWrappedHeader($"Error posting Webhook", headerColor: ConsoleColor.Red);
                    ColorConsole.WriteError(ex.Message);

                }

            }

        }

    }
}