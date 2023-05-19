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
    internal class XMatters
    {

        private RestClient restClient;
        private Models.Settings.Alerts alertSettings {get;set;}
        public XMatters(Models.Settings.Alerts alertSettings) { 
            restClient = Connect(alertSettings); //connect
            this.alertSettings = alertSettings;
        }

        private RestClient Connect(Models.Settings.Alerts alertSettings)
        {
            var options = new RestClientOptions(string.Format("https://{0}", alertSettings.xMattersBase));

            //define rest client
            var restClient = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson());

            return restClient;

        }

        public void TriggerWebhook(GenericAlert alert)
        {

            var req = new RestRequest(alertSettings.xMattersHookPath, Method.Post);
            req.AddJsonBody(new Models.XMatters.TriggerEvent()
            { 
                Description = $"{alert.Message} for {alert.Parent} on {alert.VBRServer}",
                Priority = alert.Priority,
                Recipients = alertSettings.xMattersRecipients,
                Summary = alert.Subject
            });

      
            try
            {
                var resp = restClient.Execute<dynamic>(req);
                ColorConsole.WriteWrappedHeader($"xMatters triggered - {alert.Parent} {alert.Message}.", headerColor: ConsoleColor.Green);

            }
            catch (Exception ex)
            {

                ColorConsole.WriteWrappedHeader($"xMatters failure - {alert.Parent} {alert.Message}.", headerColor: ConsoleColor.Red);
                ColorConsole.WriteError(ex.Message);

            }
        }

    }
}
