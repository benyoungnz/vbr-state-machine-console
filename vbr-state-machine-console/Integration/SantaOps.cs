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
    internal class SantaOps
    {


        private Models.Settings.Alerts alertSettings {get;set;}
        public SantaOps(Models.Settings.Alerts alertSettings) { 

            this.alertSettings = alertSettings;
        }


        public void TriggerWebhook(GenericAlert alert)
        {

            var triggerEvent = new Models.SantaOps.TriggerEvent()
            { 
                Message = $"{alert.Message} for {alert.Parent}",
                Hostname = alert.VBRServer,
                TriggeredOn = DateTime.Now.ToString(),
                Summary = alert.Subject
            };

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(triggerEvent);
                try
                {
                    var result = client.PostAsync(alertSettings.SantaOpsUri, new StringContent(json, null, "application/json")).Result;
                    var x = result;
                     ColorConsole.WriteWrappedHeader($"Santa Ops triggered - {alert.Parent} {alert.Message}.", headerColor: ConsoleColor.Green);

                }
                catch (Exception ex)
                {

                    ColorConsole.WriteWrappedHeader($"Santa Ops failure - {alert.Parent} {alert.Message}.", headerColor: ConsoleColor.Red);
                    ColorConsole.WriteError(ex.Message);

                }

            }
            // var req = new RestRequest(alertSettings.xMattersHookPath, Method.Post);
            // // req.AddJsonBody(new Models.XMatters.TriggerEvent()
            // // { 
            // //     Description = $"{alert.Message} for {alert.Parent} on {alert.VBRServer}",
            // //     Priority = alert.Priority,
            // //     Recipients = alertSettings.xMattersRecipients,
            // //     Summary = alert.Subject
            // // });
            //   req.AddJsonBody(new Models.XMatters.TriggerEvent()
            // { 
            //     Description = $"description",
            //     Priority = "MEDIUM",
            //     Recipients = "ben",
            //     Summary = "subject"
            // });

      
            // try
            // {
            //     var resp = restClient.Execute<dynamic>(req);
            //     ColorConsole.WriteWrappedHeader($"xMatters triggered - {alert.Parent} {alert.Message}.", headerColor: ConsoleColor.Green);

            // }
            // catch (Exception ex)
            // {

            //     ColorConsole.WriteWrappedHeader($"xMatters failure - {alert.Parent} {alert.Message}.", headerColor: ConsoleColor.Red);
            //     ColorConsole.WriteError(ex.Message);

            // }
        }

    }
}
