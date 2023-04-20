using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using vbr_state_machine_console.Models.VBR;
using veeam_repository_reporter;

namespace vbr_state_machine_console.Integration
{
    internal class VBR
    {

        private RestClient restClient;
        private string routeVer;
        public VBR(Models.Settings.BackupServer bkpServer) { 
            restClient = Connect(bkpServer); //connect
            routeVer = bkpServer.ApiRouteVersion;
        }

        private RestClient Connect(Models.Settings.BackupServer bkpServer)
        {
            var options = new RestClientOptions(string.Format("https://{0}:{1}", bkpServer.Host, bkpServer.Port));

            //dev only - trust self signed certs, uncomment the line below if you want to put these protection back in place
            options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            //define rest client
            var restClient = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson());


            //needed for all requests to the api
            restClient.AddDefaultHeader("x-api-version", bkpServer.ApiVersion);

            //perform the login
            var loginReq = new RestRequest("/api/oauth2/token", Method.Post);
            loginReq.AddParameter("username", bkpServer.Username);
            loginReq.AddParameter("password", bkpServer.Password);
            loginReq.AddParameter("grant_type", "password");


            ColorConsole.WriteWrappedHeader($"{bkpServer.Host}", headerColor: ConsoleColor.Cyan);

            var resp = restClient.Execute<OAuth2TokenResponse>(loginReq);

            if (resp.IsSuccessful)
            {
                if (string.IsNullOrEmpty(resp.Data.AccessToken))
                    throw new Exception("Login failed, no session token returned");
                else
                {
                    //all requests from now need to be authorized, add the default header
                    restClient.AddDefaultHeader("Authorization", "Bearer " + resp.Data.AccessToken);
                }
            }
            else
            {
                throw new ApplicationException("Login failed, no session token returned");
            }

            return restClient;

        }


        public List<Models.VBR.SOBR.ScaleoutRepository> GetSOBRS()
        {

            var req = new RestRequest("/api/{apiVersionRoute}/backupInfrastructure/scaleoutrepositories", Method.Get);

            req.AddUrlSegment("apiVersionRoute", routeVer);
            var content = restClient.Execute<Models.VBR.SOBR.ScaleoutRepositories>(req);

            return content.Data.Data;
        }
    }
}
