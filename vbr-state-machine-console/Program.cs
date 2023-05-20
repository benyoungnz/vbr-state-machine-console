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
        private static Alerts settingsAlerts { get; set; }
        private static DesiredStates settingsDesiredStates { get; set; }

        static void Main(string[] args)
        {

            //setup config
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

            //alert settings
            settingsAlerts = config.GetRequiredSection("Alerts").Get<Alerts>();

            //desired states settings
            settingsDesiredStates = config.GetRequiredSection("DesiredStates").Get<DesiredStates>();

            //monitoring
            //TBC
            //settingsDesiredStates = config.GetRequiredSection("DesiredStates").Get<Models.Settings.Monitoring>(); 

            var backupServers = config.GetRequiredSection("BackupServers").Get<List<Models.Settings.BackupServer>>();

            foreach (var backupServer in backupServers)
            {
                ProcessBackupServer(backupServer);
            }



        }


        /// <summary>
        /// Method <c>StateComparer</c> takes in values from the server instance and settings and converts them to a generic object for use in the alert modules.
        /// </summary>
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

        /// <summary>
        /// Method <c>StateCompareGeneralSettings</c> Compares global settings for desired state
        /// </summary>
        private static List<Models.AlertDestination.GenericCompare> StateCompareGeneralSettings(Integration.VBR vbrSession)
        {

            var lstStateTracking = new List<Models.AlertDestination.GenericCompare>();

            var generalOpt = vbrSession.GetGeneralOptions();
            var serverTime = vbrSession.GetServerTime();

            ColorConsole.WriteWrappedHeader($"General Options", headerColor: ConsoleColor.Green);

            //email enabled
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.EmailIsEnabled, generalOpt.EmailSettings.IsEnabled, "Email Enabled", "Global", vbrSession.server));
            //email from
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.EmailFrom, generalOpt.EmailSettings.From, "Email From", "Global", vbrSession.server));
            //email to
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.EmailTo, generalOpt.EmailSettings.To, "Email To", "Global", vbrSession.server));
            //email server
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.EmailSmtpServer, generalOpt.EmailSettings.SmtpServerName, "SMTP Server", "Global", vbrSession.server));
            //email ssl
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.EmailSslEnabled, generalOpt.EmailSettings.AdvancedSmtpOptions.SslEnabled, "SMTP SSL", "Global", vbrSession.server));
            //email port
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.EmailAdvPort, generalOpt.EmailSettings.AdvancedSmtpOptions.Port, "SMTP Port", "Global", vbrSession.server));

            //notification success
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.NotifyOnSuccess, generalOpt.EmailSettings.NotifyOnSuccess, "Notify on Success", "Global", vbrSession.server));
            //notification warning
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.NotifyOnWarning, generalOpt.EmailSettings.NotifyOnWarning, "Notify on Warning", "Global", vbrSession.server));
            //notification failure
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.NotifyOnFailure, generalOpt.EmailSettings.NotifyOnFailure, "Notify on Failure", "Global", vbrSession.server));
            //notification updates
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.NotifyOnUpdates, generalOpt.Notifications.NotifyOnUpdates, "Notify on Updates", "Global", vbrSession.server));
            //notification suport expire
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.NotifyOnSupportExpiration, generalOpt.Notifications.NotifyOnSupportExpiration, "Notify on Support Exp", "Global", vbrSession.server));


            //server time zone
            lstStateTracking.Add(StateComparer(settingsDesiredStates.GeneralOptions.ServerTimeZone, serverTime.TimeZone, "Server Time Zone", "Global", vbrSession.server));


            return lstStateTracking;

        }

        /// <summary>
        /// Method <c>StateCompareSOBR</c> Compares a SOBR against desired state
        /// </summary>
        private static List<Models.AlertDestination.GenericCompare> StateCompareSOBR(Integration.VBR vbrSession)
        {

            var lstStateTracking = new List<Models.AlertDestination.GenericCompare>();
            foreach (var sobr in vbrSession.GetSOBRS())
            {

                ColorConsole.WriteWrappedHeader($"{sobr.Name}", headerColor: ConsoleColor.Green);

                ColorConsole.WriteEmbeddedColorLine($"Capacity Tier: [green]{sobr.CapacityTier.Enabled}[/green] // Archive Tier: [green]{sobr.ArchiveTier.IsEnabled}[/green]");
                ColorConsole.WriteEmbeddedColorLine($"Placement: [yellow]{sobr.PlacementPolicy.Type}[/yellow] // ID: [yellow]{sobr.Id}[/yellow]");

                //desired state checks

                //placement policy
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrPlacementPolicy.PlacementType, sobr.PlacementPolicy.Type, "Placement Policy", sobr.Name, vbrSession.server));

                //performance tier
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrPerformanceTier.PerVmBackup, sobr.PerformanceTier.AdvancedSettings.PerVmBackup, "Per VM Backup Chains", sobr.Name, vbrSession.server));

                //capacity tier enabled
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.Enabled, sobr.CapacityTier.Enabled, "Capacity Tier Enabled", sobr.Name, vbrSession.server));
                //capacity tier immediate copy
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.ImmediateCopyRequired, sobr.CapacityTier.CopyPolicyEnabled, "Capacity Tier Encryption", sobr.Name, vbrSession.server));
                //capacity tier enforce encryption
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.EnforceEncryption, sobr.CapacityTier.Encryption.IsEnabled, "Capacity Tier Encryption", sobr.Name, vbrSession.server));
                //capacity tier move enabled
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.MoveEnabled, sobr.CapacityTier.MovePolicyEnabled, "Capacity Tier Move Enabled", sobr.Name, vbrSession.server));
                //capacity tier move after days
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.MoveAfterDays, sobr.CapacityTier.OperationalRestorePeriodDays, "Capacity Tier Move After Days", sobr.Name, vbrSession.server));


                //archive tier enabled
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.Enabled, sobr.ArchiveTier.IsEnabled, "Archive Tier Enabled", sobr.Name, vbrSession.server));
                //archive tier period days
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.ArchivePeriodDays, sobr.ArchiveTier.ArchivePeriodDays, "Archive Tier Period Days", sobr.Name, vbrSession.server));
                //archive tier cost optimized
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.CostOptimizedEnabled, sobr.ArchiveTier.AdvancedSettings.CostOptimizedArchiveEnabled, "Archive Tier Cost Optimized", sobr.Name, vbrSession.server));
                //archive tier cost optimized
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.DedupeEnabled, sobr.ArchiveTier.AdvancedSettings.ArchiveDeduplicationEnabled, "Archive Tier Dedupe", sobr.Name, vbrSession.server));

            }

            return lstStateTracking;

        }

        /// <summary>
        /// Method <c>StateCompareJobConfig</c> Compares a Job config against desired state
        /// </summary>
        private static List<Models.AlertDestination.GenericCompare> StateCompareJobConfig(Integration.VBR vbrSession)
        {
            
            var lstStateTracking = new List<Models.AlertDestination.GenericCompare>();
            foreach (var jobConfig in vbrSession.GetJobConfigs())
            {

                ColorConsole.WriteWrappedHeader($"Job: {jobConfig.Name}", headerColor: ConsoleColor.Green);

                //desired state checks
                if (settingsDesiredStates.JobConfig.Exclusions.Contains(jobConfig.Name)) //processing exclusions
                {
                    ColorConsole.WriteEmbeddedColorLine("[yellow]Job has been excluded from DSC.[/yellow]");
                }
                else
                {

                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.IsDisabled, jobConfig.IsDisabled, "Job Disabled", jobConfig.Name, vbrSession.server));

                    //Retention Policy
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.RetentionPolicy.Type, jobConfig.Storage.RetentionPolicy.Type, "Retention Policy Type", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.RetentionPolicy.Quantity, jobConfig.Storage.RetentionPolicy.Quantity, "Retention Policy Quantity", jobConfig.Name, vbrSession.server));

                    //Advanced Settings
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.BackupModeType, jobConfig.Storage.AdvancedSettings.BackupModeType, "Backup Mode", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.StorageData.CompressionLevel, jobConfig.Storage.AdvancedSettings.StorageData.CompressionLevel, "Compression Level", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.StorageData.StorageOptimization, jobConfig.Storage.AdvancedSettings.StorageData.StorageOptimization, "Storage Optimization", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.StorageData.EnableInlineDataDedup, jobConfig.Storage.AdvancedSettings.StorageData.EnableInlineDataDedup, "Storage Optimization", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.StorageData.Encryption.IsEnabled, jobConfig.Storage.AdvancedSettings.StorageData.Encryption.IsEnabled, "Job-level Encryption", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.Notifications.SendSnmpNotifications, jobConfig.Storage.AdvancedSettings.Notifications.SendSnmpNotifications, "Send SNMP Notifications", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.Notifications.EmailNotifications.IsEnabled, jobConfig.Storage.AdvancedSettings.Notifications.EmailNotifications.IsEnabled, "Email Notifications", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.VSphere.EnableVmwareToolsQuiescence, jobConfig.Storage.AdvancedSettings.VSphere.EnableVmwareToolsQuiescence, "VMware Tools Quiescence", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.VSphere.ChangedBlockTracking.IsEnabled, jobConfig.Storage.AdvancedSettings.VSphere.ChangedBlockTracking.IsEnabled, "VMware CBT", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.VSphere.ChangedBlockTracking.EnableCbtautomatically, jobConfig.Storage.AdvancedSettings.VSphere.ChangedBlockTracking.EnableCbtautomatically, "Enable CBT Automatically", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.VSphere.ChangedBlockTracking.ResetCbtonActiveFull, jobConfig.Storage.AdvancedSettings.VSphere.ChangedBlockTracking.ResetCbtonActiveFull, "Reset CBT on Active Full", jobConfig.Name, vbrSession.server));
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.JobConfig.Storage.AdvancedSettings.StorageIntegration.IsEnabled, jobConfig.Storage.AdvancedSettings.StorageIntegration.IsEnabled, "Storage Integration", jobConfig.Name, vbrSession.server));

                }

            }

            return lstStateTracking;
            
        }

        // /// <summary>
        // /// Method <c>StateCompareJobState</c> Compares a Job status against desired state
        // /// </summary>
        // private static List<Models.AlertDestination.GenericCompare> StateCompareJobState(Integration.VBR vbrSession)
        // {
        //     foreach (var jobState in vbrSession.GetJobStates())
        //     {


        //         ColorConsole.WriteWrappedHeader($"{jobState.Name}", headerColor: ConsoleColor.Green);

        //         ColorConsole.WriteEmbeddedColorLine($"Status: [green]{jobState.Status}[/green]");
        //         ColorConsole.WriteEmbeddedColorLine($"Last Run: [green]{jobState.LastRun}[/green]");

        //         //desired state checks

        //         //capacity tier enabled check
        //         // lstStateTracking.Add(StateComparer(settingsDesiredStates.CapacityTier.Enabled, sobr.CapacityTier.Enabled, "Capacity Tier Enabled", sobr.Name, bkpServer));
                
        //     }
        // }

        private static void ProcessBackupServer(BackupServer bkpServer)
        {
            //fire up a VBR session
            var vbrSession = new Integration.VBR(bkpServer);


            //globals for alerts and keeping track of state 
            var lstStateTracking = new List<Models.AlertDestination.GenericCompare>();



            //global settings
            lstStateTracking.AddRange(StateCompareGeneralSettings(vbrSession));

            //scale out backup repositories
            lstStateTracking.AddRange(StateCompareSOBR(vbrSession));

            //job configs
            lstStateTracking.AddRange(StateCompareJobConfig(vbrSession));

            //job states
            // lstStateTracking.AddRange(StateCompareJobState(vbrSession));


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