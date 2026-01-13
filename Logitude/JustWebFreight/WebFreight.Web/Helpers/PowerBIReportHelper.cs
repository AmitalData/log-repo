using Microsoft.Identity.Client;
using Microsoft.Rest;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using System;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.IO;
using System.Web;


namespace WebFreight.Web.Helpers
{
    public class PowerBIReportHelper
    {
        private int Tenant { get; set; }
        public string AccessToken { get; set; }
        public string ActiveDirectoryTenantId { get; set; }
        private readonly string SetKey = "PowerBIParams";
        List<DefaultAndConfigurationPM> Config;

        public PowerBIReportHelper(int tenant)
        {
            Tenant = tenant;
            ActiveDirectoryTenantId = GetConfig("ActiveDirectoryTenantId").Value1;

            GenerateAccessToken();
        }

        public List<ReportResult> GetReports()
        {
            string workspaceId = GetConfig("WorkspaceId").Value1;
            if (string.IsNullOrEmpty(workspaceId))
            {
                throw new Exception("PowerBI WorkspaceId is not configured.");
            }
            return GetPowerBIWorkspaceReports(Guid.Parse(workspaceId), true);
        }

        private List<ReportResult> GetPowerBIWorkspaceReports(System.Guid workspaceId, bool generateEmbedToken = false)
        {
            List<ReportResult> reportResults = new List<ReportResult>();
            var tokenCredentials = new TokenCredentials(AccessToken, "Bearer");

            using (var client = new PowerBIClient(new Uri("https://api.powerbi.com/"), tokenCredentials))
            {
                var reports = client.Reports.GetReportsInGroup(workspaceId);
                foreach (var report in reports.Value)
                {
                    reportResults.Add(new ReportResult()
                    {
                        Id = report.Id.ToString(),
                        Name = report.Name,
                        EmbedUrl = report.EmbedUrl,
                        EmbedToken = generateEmbedToken? GenerateEmbedToken(workspaceId, report.Id.ToString()): null
                    });

                    //string exportId = StartExport(workspaceId, report.Id.ToString());
                    //string downloadUrl = WaitForExport(workspaceId, report.Id.ToString(), exportId);
                    //DownloadPdf(downloadUrl, @"C:\temp\report.pdf");
                }
            }
            return reportResults;
        }


        private string StartExport(Guid workspaceId, string reportId)
        {
            string url = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}/ExportTo";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var body = new StringContent(
                    "{\"format\":\"PDF\"}",
                    Encoding.UTF8,
                    "application/json");

                var response = client.PostAsync(url, body).Result;

                if (response.StatusCode != HttpStatusCode.Accepted)
                    throw new Exception(response.Content.ReadAsStringAsync().Result);

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                return result.id;
            }
        }

        private string WaitForExport(Guid workspaceId, string reportId, string exportId)
        {
            string url = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}/exports/{exportId}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                while (true)
                {
                    var response = client.GetAsync(url).Result;
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                    string status = result.status;

                    if (status == "Succeeded")
                        return result.resourceLocation;

                    if (status == "Failed")
                        throw new Exception("Export failed");

                    System.Threading.Thread.Sleep(2000);
                }
            }
        }

        private void DownloadPdf(string resourceUrl, string filePath)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var bytes = client.GetByteArrayAsync(resourceUrl).Result;
                File.WriteAllBytes(filePath, bytes);
            }
        }

        private void GenerateAccessToken()
        {
            string clientId = GetConfig("ClientId").Value1;
            string clientSecret = GetConfig("ClientSecret").Value1;
            try
            {
                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                {
                    throw new Exception("PowerBI ClientId or ClientSecret is not configured.");
                }
                string[] scopes = new string[] { "https://analysis.windows.net/powerbi/api/.default" };

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority($"https://login.microsoftonline.com/{ActiveDirectoryTenantId}")
                    .Build();

                AuthenticationResult result = app.AcquireTokenForClient(scopes).ExecuteAsync().Result;
                AccessToken = result.AccessToken;
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed to generate access token for client id: {clientId}, error: {ex.Message}");
                throw new Exception($"Failed to generate access token for PowerBI with client id: {clientId}", ex);
            }
        }

        private DefaultAndConfigurationPM GetConfig(string additionalKey)
        {
            try
            {
                if (Config == null)
                {
                    DefaultAndConfigurationQuery defaultAndConfigurationQuery = new DefaultAndConfigurationQuery(Tenant);
                    Config = defaultAndConfigurationQuery.GetIQueryableDefaultAndConfigurationsPMBySetKey(SetKey).Where(p => p.Tenant == Tenant).ToList();
                }

                DefaultAndConfigurationPM defaultConfig = Config.Where(a => string.Equals(a.AdditionalKey, additionalKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (defaultConfig == null)
                {
                    throw new Exception($"default not found for SetKey: {SetKey}, additionalKey: {additionalKey}");
                }
                return defaultConfig;
            }
            catch (Exception ex)
            {
                if (HttpContext.Current.Request.Url.Host == "localhost")
                {
                    switch (additionalKey)
                    {
                        case "ClientId":
                            return new DefaultAndConfigurationPM() { Value1 = "43e613a9-5088-4696-9437-07977da79abc" };
                        case "ClientSecret":
                            return new DefaultAndConfigurationPM() { Value1 = "YBL8Q~hf6Qpdf_1akbG5RQxkzSQTiRZ1wCWAXcMh" };
                        case "ActiveDirectoryTenantId":
                            return new DefaultAndConfigurationPM() { Value1 = "97e9eb66-9e26-46fa-8f77-1cdbaf9a9649" };
                        case "WorkspaceId":
                            return new DefaultAndConfigurationPM() { Value1 = "492b6a52-a1ff-4543-b441-d56e5a530e40" };
                    }
                }
                NetCommonHelper.Logger.DevLog.Instance.WriteWarning($"Not found default for SetKey: {SetKey}, additionalKey: {additionalKey}, error: {ex.Message}");
                throw new Exception($"Not found default for SetKey: {SetKey}, additionalKey: {additionalKey}. Please check it is defined.", ex);
            }
        }

        public string GenerateEmbedToken(Guid workspaceId, string reportId)
        {
            string url = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}/GenerateToken";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                var body = new StringContent("{\"accessLevel\":\"View\"}", Encoding.UTF8, "application/json");

                var response = client.PostAsync(url, body).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var error = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"GenerateToken failed: {response.StatusCode} - {error}");
                }

                var json = response.Content.ReadAsStringAsync().Result;
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                return result.token;
            }
        }
    }

    public class ReportResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmbedUrl { get; set; }
        public string EmbedToken { get; set; }
    }
}
