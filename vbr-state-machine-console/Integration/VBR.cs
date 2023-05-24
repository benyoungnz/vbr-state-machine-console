using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using vbr_state_machine_console.Models.Settings;
using vbr_state_machine_console.Models.VBR;
using veeam_repository_reporter;

namespace vbr_state_machine_console.Integration
{
    internal class VBR
    {

        private RestClient restClient;
        private string routeVer;
        private double limit;
        public BackupServer server;
        public string serverHostname;
        public VBR(Models.Settings.BackupServer bkpServer) { 
            restClient = Connect(bkpServer); //connect
            routeVer = bkpServer.ApiRouteVersion;
            limit = bkpServer.ApiLimit;
            server = bkpServer;
            serverHostname = bkpServer.Host;
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

        public Models.VBR.General.GeneralOptions GetGeneralOptions()
        {
            var req = new RestRequest("/api/{apiVersionRoute}/generalOptions", Method.Get);
            req.AddUrlSegment("apiVersionRoute", routeVer);
      

            var content = restClient.Execute<Models.VBR.General.GeneralOptions>(req);

            return content.Data;
        }

        public Models.VBR.General.GeneralOptions PutGeneralOptions(Models.VBR.General.GeneralOptions putOptions)
        {
            var req = new RestRequest("/api/{apiVersionRoute}/generalOptions", Method.Put);
            req.AddUrlSegment("apiVersionRoute", routeVer);
            
            req.AddJsonBody(putOptions); //updated data.

            var content = restClient.Execute<Models.VBR.General.GeneralOptions>(req);

            return content.Data;
        }

        public Models.VBR.General.ServerTime GetServerTime()
        {
            var req = new RestRequest("/api/{apiVersionRoute}/serverTime", Method.Get);
            req.AddUrlSegment("apiVersionRoute", routeVer);


            var content = restClient.Execute<Models.VBR.General.ServerTime>(req);

            return content.Data;
        }

        public List<Models.VBR.BackupInfrastructure.ScaleoutRepository> GetSOBRS()
        {
            var req = new RestRequest("/api/{apiVersionRoute}/backupInfrastructure/scaleoutrepositories", Method.Get);
            req.AddUrlSegment("apiVersionRoute", routeVer);
            req.AddParameter("limit", limit);

            var content = restClient.Execute<Models.VBR.BackupInfrastructure.ScaleoutRepositories>(req);

            // Is there more than 1 page of results?
            if (content.Data.Pagination.Count < content.Data.Pagination.Total) {
                // Determine page count
	            double pageTotal = content.Data.Pagination.Total / content.Data.Pagination.Count;
                pageTotal = Math.Ceiling(pageTotal);
                var page = 0;
                
                while (page != pageTotal) {
                    page++;
		            var skip = page * limit;
                    
                    req.AddOrUpdateParameter("skip", skip);
                    var pagedResponse = restClient.Execute<Models.VBR.BackupInfrastructure.ScaleoutRepositories>(req);
                    
                    content.Data.Data.AddRange(pagedResponse.Data.Data);
                }
            }

            return content.Data.Data;
        }

        public List<Models.VBR.BackupInfrastructure.RepositoryState> GetRepoStates()
        {

            var req = new RestRequest("/api/{apiVersionRoute}/backupInfrastructure/repositories/states", Method.Get);
            req.AddUrlSegment("apiVersionRoute", routeVer);
            req.AddParameter("limit", limit);
            
             var content = restClient.Execute<Models.VBR.BackupInfrastructure.RepositoryStates>(req);

            // Is there more than 1 page of results?
            if (content.Data.Pagination.Count < content.Data.Pagination.Total) {
                // Determine page count
	            double pageTotal = content.Data.Pagination.Total / content.Data.Pagination.Count;
                pageTotal = Math.Ceiling(pageTotal);
                var page = 0;
                
                while (page != pageTotal) {
                    page++;
		            var skip = page * limit;
                    
                    req.AddOrUpdateParameter("skip", skip);
                    var pagedResponse = restClient.Execute<Models.VBR.BackupInfrastructure.RepositoryStates>(req);
                    
                    content.Data.Data.AddRange(pagedResponse.Data.Data);
                }
            }

            return content.Data.Data;
        }


        public List<Models.VBR.Job.State> GetJobStates()
        {
            var req = new RestRequest("/api/{apiVersionRoute}/jobs/states", Method.Get);
            req.AddUrlSegment("apiVersionRoute", routeVer);
            req.AddParameter("limit", limit);

            var content = restClient.Execute<Models.VBR.Job.States>(req);

            // Is there more than 1 page of results?
            if (content.Data.Pagination.Count < content.Data.Pagination.Total) {
                // Determine page count
	            double pageTotal = content.Data.Pagination.Total / content.Data.Pagination.Count;
                pageTotal = Math.Ceiling(pageTotal);
                var page = 0;
                
                while (page != pageTotal) {
                    page++;
		            var skip = page * limit;
                    
                    req.AddOrUpdateParameter("skip", skip);
                    var pagedResponse = restClient.Execute<Models.VBR.Job.States>(req);
                    
                    content.Data.Data.AddRange(pagedResponse.Data.Data);
                }
            }

            return content.Data.Data;
        }

        public List<Models.VBR.Job.Configuration> GetJobConfigs()
        {
            var req = new RestRequest("/api/{apiVersionRoute}/jobs", Method.Get);
            req.AddUrlSegment("apiVersionRoute", routeVer);
            req.AddParameter("limit", limit);

            var content = restClient.Execute<Models.VBR.Job.Configurations>(req);

            // Is there more than 1 page of results?
            if (content.Data.Pagination.Count < content.Data.Pagination.Total) {
                // Determine page count
	            double pageTotal = content.Data.Pagination.Total / content.Data.Pagination.Count;
                pageTotal = Math.Ceiling(pageTotal);
                var page = 0;
                
                while (page != pageTotal) {
                    page++;
		            var skip = page * limit;
                    
                    req.AddOrUpdateParameter("skip", skip);
                    var pagedResponse = restClient.Execute<Models.VBR.Job.Configurations>(req);
                    
                    content.Data.Data.AddRange(pagedResponse.Data.Data);
                }
            }

            return content.Data.Data;
        }
    }
}
