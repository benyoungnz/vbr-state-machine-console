using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using vbr_state_machine_console.Integration;
using vbr_state_machine_console.Models.AlertDestination;
using vbr_state_machine_console.Models.Settings;
using vbr_state_machine_console.Models.VBR;
using veeam_repository_reporter;

namespace vbr_state_machine_console
{


    internal class Program
    {
        private static Monitors settingsMonitors { get; set; }
        private static Alerts settingsAlerts { get; set; }
        private static DesiredStates settingsDesiredStates { get; set; }
        private static List<GenericAlert> alertsToTrigger { get; set; }

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
            settingsMonitors = config.GetRequiredSection("Monitors").Get<Monitors>();
            alertsToTrigger = new List<GenericAlert>();

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
                gc.Description = $"As expected ({gc.Expected})";
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

            var repoStates = vbrSession.GetRepoStates();

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

                if (sobr.CapacityTier.Enabled) //only check these if enabled
                {
                    //capacity tier immediate copy
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.ImmediateCopyRequired, sobr.CapacityTier.CopyPolicyEnabled, "Capacity Tier Encryption", sobr.Name, vbrSession.server));
                    //capacity tier enforce encryption
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.EnforceEncryption, sobr.CapacityTier.Encryption.IsEnabled, "Capacity Tier Encryption", sobr.Name, vbrSession.server));
                    //capacity tier move enabled
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.MoveEnabled, sobr.CapacityTier.MovePolicyEnabled, "Capacity Tier Move Enabled", sobr.Name, vbrSession.server));
                    //capacity tier move after days
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrCapacityTier.MoveAfterDays, sobr.CapacityTier.OperationalRestorePeriodDays, "Capacity Tier Move After Days", sobr.Name, vbrSession.server));
                }


                //archive tier enabled
                lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.Enabled, sobr.ArchiveTier.IsEnabled, "Archive Tier Enabled", sobr.Name, vbrSession.server));
                if (sobr.ArchiveTier.IsEnabled) // only check these if enabled
                {
                    //archive tier period days
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.ArchivePeriodDays, sobr.ArchiveTier.ArchivePeriodDays, "Archive Tier Period Days", sobr.Name, vbrSession.server));
                    //archive tier cost optimized
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.CostOptimizedEnabled, sobr.ArchiveTier.AdvancedSettings.CostOptimizedArchiveEnabled, "Archive Tier Cost Optimized", sobr.Name, vbrSession.server));
                    //archive tier cost optimized
                    lstStateTracking.Add(StateComparer(settingsDesiredStates.SobrArchiveTier.DedupeEnabled, sobr.ArchiveTier.AdvancedSettings.ArchiveDeduplicationEnabled, "Archive Tier Dedupe", sobr.Name, vbrSession.server));
                }


                //monitoring 
                //get state information, such as utilisation
                //perf extents this SOBR (there may be more than one)
                ColorConsole.WriteEmbeddedColorLine($"\n[green]Performance Extent Monitoring[/green]");
                foreach (var pe in sobr.PerformanceTier.PerformanceExtents)
                {
                    ColorConsole.WriteEmbeddedColorLine($"Name: [yellow]{pe.Name}[/yellow] // Status: [yellow]{pe.Status}[/yellow]");
                    var perfRepoState = repoStates.Where(x => x.Id == pe.Id).FirstOrDefault();
                    ColorConsole.WriteInfo("\nState Information:");
                    if (perfRepoState != null)
                    {
                        var perfCapacityPercentUsed = CalculatePercent(perfRepoState.UsedSpaceGb, perfRepoState.CapacityGb);


                        if (perfCapacityPercentUsed >= settingsMonitors.SobrPerformanceTier.GbWarningLevel)
                        {
                            var priority = perfCapacityPercentUsed >= settingsMonitors.SobrPerformanceTier.GbCriticalLevel ? "MEDIUM" : "CRITICAL";
                            alertsToTrigger.Add(new GenericAlert()
                            {
                                Message = $"Capacity: {perfRepoState.CapacityGb}gb, Used: {perfRepoState.UsedSpaceGb}gb",
                                Parent = sobr.Name,
                                VBRServer = vbrSession.serverHostname,
                                Property = $"Performance Extent {pe.Name}",
                                Subject = $"Performance extent at {perfCapacityPercentUsed}%"

                            });
                            ColorConsole.WriteEmbeddedColorLine($"Status: [yellow]{priority} {perfCapacityPercentUsed}% Used[/yellow] // Name: [yellow]{pe.Name}[/yellow]");


                        }
                        else
                            ColorConsole.WriteEmbeddedColorLine($"Status: [green]OK {perfCapacityPercentUsed}% Used[/green] // Name: [green]{pe.Name}[/green]");


                    }
                }

                if (sobr.CapacityTier.Enabled)
                {
                    //capacity tier for this SOBR
                    ColorConsole.WriteEmbeddedColorLine($"\n[green]Capacity Tier Monitoring[/green]");

                    ColorConsole.WriteInfo("\nCapacity Tier //");

                    //get state information, such as utilisation
                    var capRepoState = repoStates.Where(x => x.Id == sobr.CapacityTier.ExtentId).FirstOrDefault();
                    ColorConsole.WriteLine("State Information:");
                    if (capRepoState != null)
                    {

                        if (capRepoState.UsedSpaceGb >= settingsMonitors.SobrCapacityTier.GbWarningLevel)
                        {
                            var priority = capRepoState.UsedSpaceGb >= settingsMonitors.SobrCapacityTier.GbCriticalLevel ? "MEDIUM" : "CRITICAL";
                            alertsToTrigger.Add(new GenericAlert()
                            {
                                Message = $"Used: {capRepoState.UsedSpaceGb}gb, Endpoint: {capRepoState.Path}",
                                Parent = sobr.Name,
                                VBRServer = vbrSession.serverHostname,
                                Property = $"{capRepoState.Path}",
                                Subject = $"Capacity tier usage at {capRepoState.UsedSpaceGb}gb"

                            });

                            ColorConsole.WriteEmbeddedColorLine($"Status: [yellow]{priority} {capRepoState.UsedSpaceGb}gb Used[/yellow] // Path: [yellow]{capRepoState.Path}[/yellow]");

                        }
                        else
                            ColorConsole.WriteEmbeddedColorLine($"Status: [green]OK {capRepoState.UsedSpaceGb}gb Used[/green] // Path: [green]{capRepoState.Path}[/green]");


                    }


                }

            }

            return lstStateTracking;

        }

        /// <summary>
        /// Method <c>StateCompareJobConfig</c> Compares a Job config against desired state
        /// </summary>
        private static List<Models.AlertDestination.GenericCompare> StateCompareJobConfig(Integration.VBR vbrSession)
        {

            var jobStates = vbrSession.GetJobStates();

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

                //monitoring checks
                var jobState = jobStates.Where(x => x.Id == jobConfig.Id).FirstOrDefault();
                if (jobState != null)
                {
                    if (jobState.LastResult == "Success")
                    {
                        ColorConsole.WriteEmbeddedColorLine($"Last Result: [green]{jobState.LastResult}[/green]");
                    }
                    else if (jobState.LastResult == "Warning")
                    {
                        ColorConsole.WriteEmbeddedColorLine($"Last Result: [yellow]{jobState.LastResult}[/yellow]");
                        alertsToTrigger.Add(new GenericAlert()
                        {
                            Message = "Last job run was not successful.",
                            Parent = jobState.Name,
                            VBRServer = vbrSession.serverHostname,
                            Property = $"{jobState.LastResult}",
                            Subject = $"Jobs status {jobState.LastResult}"

                        });
                    }
                    else if (jobState.LastResult == "Failed")
                    {
                        ColorConsole.WriteEmbeddedColorLine($"Last Result: [red]{jobState.LastResult}[/red]");
                        alertsToTrigger.Add(new GenericAlert()
                        {
                            Message = "Last job run was not successful.",
                            Parent = jobState.Name,
                            VBRServer = vbrSession.serverHostname,
                            Property = $"{jobState.LastResult}",
                            Subject = $"Jobs status {jobState.LastResult}"

                        });
                    }
                    else if (jobState.LastResult == "None")
                    {
                        ColorConsole.WriteEmbeddedColorLine($"Last Result: [blue]{jobState.LastResult}[/blue]");
                    }

                    if (jobState.LastRun != null)
                    {
                        DateTimeOffset now = DateTimeOffset.Now;
                        double RpoDays = (now - jobState.LastRun.Value).TotalDays;

                        if (RpoDays >= settingsMonitors.Job.RpoDays)
                        {
                            ColorConsole.WriteEmbeddedColorLine($"Days since last job run: [yellow]{Math.Floor(RpoDays)}[/yellow]");
                            ColorConsole.WriteEmbeddedColorLine($"Last Result: [red]{jobState.LastResult}[/red]");
                            alertsToTrigger.Add(new GenericAlert()
                            {
                                Message = $"Job has not run for {Math.Floor(RpoDays)} days.",
                                Parent = jobState.Name,
                                VBRServer = vbrSession.serverHostname,
                                Property = $"{jobState.Name}",
                                Subject = $"RPO ({settingsMonitors.Job.RpoDays} days) not being met!"

                            });
                        }
                        else
                            ColorConsole.WriteEmbeddedColorLine($"Less than [green]{settingsMonitors.Job.RpoDays}[/green] days since last job run.");

                    }
                    else
                    {
                        ColorConsole.WriteEmbeddedColorLine($"[green]Job has never run.[/green]");
                    }


                }

            }

            return lstStateTracking;

        }

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


            //setup our integration clients
            var xMatters = new XMatters(settingsAlerts);
            var santaOps = new SantaOps(settingsAlerts);
            var microsoftTeams = new Teams(settingsAlerts);


            //alerts
            foreach (var alert in alertsToTrigger)
            {
                if (settingsAlerts.XMattersEnabled)
                    xMatters.TriggerWebhook(alert);

                if (settingsAlerts.SantaOpsEnabled)
                    santaOps.TriggerWebhook(alert);

            }

            if (settingsAlerts.TeamsEnabled)
                microsoftTeams.TriggerAlerts(alertsToTrigger, bkpServer.Host);


            //enable teams push of state config
            if (settingsAlerts.TeamsEnabled)
                microsoftTeams.TriggerDesiredState(lstStateTracking, bkpServer.Host);








        }




        public static int CalculatePercent(double used, double capacity)
        {
            return (int)Math.Round((double)(100 * used) / capacity);
        }

    }
}