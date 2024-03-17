using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.ExportServer
{
    public class ExportServerLogin
    {
        private static Dictionary<string, string> tokensCache = new Dictionary<string, string>();
        private static TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
        public readonly static string exportUrl = new SettingQuery().GetSinglePMFromCahche().ExportUrl;

        public static string GetLinkToLogin(int tenant, string email, string exportToken)
        {
            int exportTenant = tenantManagementQuery.GetSinglePM(tenant).ExportTenant.Value;
            UserPM userPM = new UserQuery(tenant).GetSingleUserPMByEmail(email, tenant, true);
            if(userPM == null)
                userPM = new UserQuery(tenant).GetSingleUserPMByEmail(email, 0, true);

            string userCode = userPM.Code ?? userPM.EnglishName;
            string link = $"{exportUrl}/AmitalSSOAngular.html?token={exportToken}&tenant={exportTenant}&AmitalSSOAngular=1&userCode={userCode}";
            return link;
        }

        public static string GetToken(int tenant, string email)
        {
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
            if (string.IsNullOrEmpty(tenantManagementPM.ExportLoginCredintial) || tenantManagementPM.ExportTenant == null)
                throw new Exception("not config in table tenant managment field export tenant or Export Login Credintial for tenant " + tenant);

            int exportTenant = tenantManagementPM.ExportTenant.Value;
            string tokenKey = GetTokenKey(exportTenant, email);

            if (!tokensCache.ContainsKey(tokenKey))
            {
                string token = RequestTokenFromCustoms(exportTenant, email, tenantManagementPM.ExportLoginCredintial);

                if (!CheckIfUserActive(token, exportTenant))
                    return null;

                tokensCache[tokenKey] = token;
            }

            return tokensCache[tokenKey];
        }

        private static string RequestTokenFromCustoms(int exportTenant, string email, string exportLoginCredintial)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            APICredentialsParameters aPICredentialsParameters = new APICredentialsParameters() { PrimaryKey = exportLoginCredintial, Tenant = exportTenant };
            ChannelFactory<ILoginWcfService> factory = new ChannelFactory<ILoginWcfService>(
                exportUrl.StartsWith("https") ? (Binding)new BasicHttpsBinding() : (Binding)new BasicHttpBinding(),
                new EndpointAddress(exportUrl + "/WcfApi/LoginWcfService.svc"));
            ILoginWcfService channel = factory.CreateChannel();
            Response result = channel.LoginByCredential(email, aPICredentialsParameters);

            if (result.HasError)
                throw new Exception("enerror accord when try get token from export server, err: " + result.ErrorMessage);

            if (string.IsNullOrEmpty(result.Result))
                throw new Exception("enerror accord when try get token from export server, the token result return empty");

            return result.Result;
        }

        private static bool CheckIfUserActive(string token, int exportTenant)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;            
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new {Tenant = exportTenant, Token = token }), Encoding.UTF8, "application/json");
            HttpResponseMessage res = client.PostAsync(exportUrl + "/api/authentication?dummy=user", stringContent).Result;
            string content = res.Content.ReadAsStringAsync().Result;
            client.Dispose();

            UserData userData = JsonConvert.DeserializeObject<UserData>(content);
            return !userData.HasError;
        }

        private static string GetTokenKey(int tenant, string email) => tenant + ";" + email;
    }
}