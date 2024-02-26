using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WebFreight.Web.Helpers.ExportServer
{
    public class ExportServerLogin
    {
        private static Dictionary<string, string> tokensCache = new Dictionary<string, string>();
        private static TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
        public readonly static string exportUrl = new SettingQuery().GetSinglePMFromCahche().ExportUrl;

        public static string GetLinkToLogin(int tenant, string email)
        {   
            string token = GetToken(tenant, email);
            int? exportTenant = tenantManagementQuery.GetSinglePM(tenant).ExportTenant;
            string link = $"{exportUrl}/AmitalSSOAngular.html?token={token}&tenant={exportTenant.Value}&AmitalSSOAngular=1";
            return link;
        }

        public static string GetToken(int tenant, string email)
        {
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);            
            if (string.IsNullOrEmpty(tenantManagementPM.ExportLoginCredintial) || tenantManagementPM.ExportTenant == null)
                throw new Exception("not config in table tenant managment field export tenant or Export Login Credintial for tenant " + tenant);

            string tokenKey = GetTokenKey(tenantManagementPM.ExportTenant.Value, email);
            if (!tokensCache.ContainsKey(tokenKey))
                initToken(tenantManagementPM.ExportTenant.Value, email, tenantManagementPM.ExportLoginCredintial);

            return tokensCache[tokenKey];
        }

        private static void initToken(int tenant, string email, string exportLoginCredintial)
        {            
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            APICredentialsParameters aPICredentialsParameters = new APICredentialsParameters() { PrimaryKey = exportLoginCredintial, Tenant = tenant };
            ChannelFactory<ILoginWcfService> factory = new ChannelFactory<ILoginWcfService>(
                exportUrl.StartsWith("https") ? (Binding)new BasicHttpsBinding() : (Binding)new BasicHttpBinding(), 
                new EndpointAddress(exportUrl + "/WcfApi/LoginWcfService.svc"));
            ILoginWcfService channel = factory.CreateChannel();
            Response result = channel.LoginByCredential(email, aPICredentialsParameters);

            if (result.HasError)
                throw new Exception("enerror accord when try get token from export server, err: " + result.ErrorMessage);

            if (string.IsNullOrEmpty(result.Result))
                throw new Exception("enerror accord when try get token from export server, the token result return empty");

            tokensCache[GetTokenKey(tenant, email)] = result.Result;
        }
        
        private static string GetTokenKey(int tenant, string email) => tenant + ";" + email;
    }
}