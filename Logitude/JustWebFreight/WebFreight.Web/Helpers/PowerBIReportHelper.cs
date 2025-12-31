using Microsoft.Identity.Client;
using Microsoft.Rest;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using System;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Linq.Dynamic.Core;
using System.Linq;


namespace WebFreight.Web.Helpers
{
    public class PowerBIReportHelper
    {
        private int Tenant { get; set; }
        private string AccessToken { get; set; }
        public string ActiveDirectoryTenantId { get; set; }
        private readonly string SetKey = "PowerBIParams";
        List<DefaultAndConfigurationPM> Config;

        public PowerBIReportHelper(int tenant)
        {
            Tenant = tenant;
            ActiveDirectoryTenantId = GetConfig("Connection").Value1;

            GenerateAccessToken();
        }

        public List<ReportResult> GetReports()
        {
            string workspaceId = GetConfig("Connection").Value2;
            if (string.IsNullOrEmpty(workspaceId))
            {
                throw new Exception("PowerBI WorkspaceId is not configured.");
            }
            return GetPowerBIWorkspaceReports(Guid.Parse(workspaceId));
        }

        private List<ReportResult> GetPowerBIWorkspaceReports(System.Guid workspaceId)
        {
            List<ReportResult> reportResults = new List<ReportResult>();
            if (string.IsNullOrEmpty(AccessToken))
            {
                //todo: remove
                return new List<ReportResult>()
                {
                    new ReportResult() { Id = "f1a21680-ddd1-4a69-8428-87fae5b5e386", Name = "DEMO BY REPORT" },
                    new ReportResult() { Id = "f1a21680-ddd1-4a69-8428-87fae5b5e386", Name = "DEMO 2" }
                };
                throw new Exception("missing PowerBI AccessToken");
            }

            var tokenCredentials = new TokenCredentials(AccessToken, "Bearer");

            using (var client = new PowerBIClient(new Uri("https://api.powerbi.com/"), tokenCredentials))
            {
                var reports = client.Reports.GetReportsInGroup(workspaceId);
                foreach (var report in reports.Value)
                {
                    reportResults.Add(new ReportResult() { Id = report.Id.ToString(), Name = report.Name });
                }
            }
            return reportResults;
        }

        private void GenerateAccessToken()
        {
            string clientId = GetConfig("Client").Value1;
            string clientSecret = GetConfig("Client").Value2;
            try
            {
                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                {
                    throw new Exception("PowerBI ClientId or ClientSecret is not configured.");
                }
                string[] scopes = new string[] { "https://analysis.windows.net/powerbi/api/.default" };

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
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed generate access token for client id: {clientId} : {ex.Message}");
                // todo: throw;
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

                return Config.Where(a => string.Equals(a.AdditionalKey, additionalKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // todo: remove
                return new DefaultAndConfigurationPM()
                {
                    Value1 = "82f78eb1-51f0-4e08-ac9c-36d05044baf6", // ActiveDirectoryTenantId
                    Value2 = "f1a21680-ddd1-4a69-8428-87fae5b5e386"  // WorkspaceId
                };
                throw new Exception($"Failed to get PowerBI configuration for SetKey: {SetKey}, additionalKey: {additionalKey}", ex);
            }
        }
    }

    public class ReportResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
