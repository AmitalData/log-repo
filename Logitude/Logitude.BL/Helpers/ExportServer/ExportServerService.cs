using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Simplog.Global.Data.GlobalModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using System.Windows.Interop;

namespace Logitude.BL.Helpers.ExportServer
{
    public class ExportServerService
    {
        private static TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
        private static TenantManagementService tenantManagementService = new TenantManagementService(GlobalContext.GetContext());

        public static ExportServerSettings GetSettings(int tenant)
        {
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
            return new ExportServerSettings()
            {
                exportLoginCredential = tenantManagementPM.ExportLoginCredintial,
                exportTenant = tenantManagementPM.ExportTenant,
            };
        }

        public static void UpdateSettings(int tenant, ExportServerSettings exportServerSettings)
        {
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
            tenantManagementPM.ExportLoginCredintial = exportServerSettings.exportLoginCredential;
            tenantManagementPM.ExportTenant = exportServerSettings.exportTenant;
            tenantManagementService.Update(tenantManagementPM, true);
        }

        public static ApiResponse CreateConfirmationNumber(int tenant, string email, string body)
        {
            string exportToken = GetTokenForConfirmationNumber(tenant, email);
            string urlCreateConfirmationNumber = ExportServerLogin.exportUrl + "/api/ShaamWebService/createConfirmationNumber";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Token", exportToken);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            StringContent stringContent = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage res = client.PostAsync(urlCreateConfirmationNumber, stringContent).Result;
            ApiResponse apiResponse = new ApiResponse()
            {
                Res = res,
                Msg = res.Content.ReadAsStringAsync().Result
            };
            client.Dispose();
            return apiResponse;
        }

        public static string GetTokenForConfirmationNumber(int tenant, string email)
        {

            string exportToken = ExportServerLogin.GetToken(tenant, email);

            if (exportToken == null)
            {
                int? exportTenant = tenantManagementQuery.GetSinglePM(tenant).ExportTenant;
                exportToken = ExportServerLogin.GetToken(tenant, $"system@tenant{exportTenant}.com");
            }

            return exportToken;

        }
    }

    public class ExportServerSettings
    {
        public string exportLoginCredential { get; set; }
        public int? exportTenant { get; set; }
    }

    public class ApiResponse
    {
        public HttpResponseMessage Res { get; set; }
        public string Msg { get; set; }
    }
}