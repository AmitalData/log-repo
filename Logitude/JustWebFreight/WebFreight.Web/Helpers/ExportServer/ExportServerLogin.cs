using Logitude.BL.GlobalModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WebFreight.Web.Helpers.ExportServer
{
    public class ExportServerLogin
    {
        private static Dictionary<string, string> tokens = new Dictionary<string, string>();
        private static TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
        private static string exportUrl = new SettingQuery().GetSinglePMFromCahche().ExportUrl;

        public static string GetLinkToLogin(int tenant, string email)
        {
            int exportTenant = tenantManagementQuery.GetTenantManagementPM(tenant).ExportTenant;

            string token = GetToken(exportTenant, email);
            string link = $"{exportUrl}/AmitalSSOAngular.html?token={token}&tenant={exportTenant}&AmitalSSOAngular=1";
            return link;
        }

        private static string GetToken(int tenant, string email)
        {
            string tokenKey = GetTokenKey(tenant, email);
            if (!tokens.ContainsKey(tokenKey))
                initToken(tenant, email);

            return tokens[tokenKey];
        }

        private static void initToken(int tenant, string email)
        {
            string primaryKey = tenantManagementQuery.GetTenantManagementPM(tenant).ExportLoginCredintial;
            APICredentialsParameters aPICredentialsParameters = new APICredentialsParameters() { PrimaryKey = primaryKey, Tenant = tenant };
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress address = new EndpointAddress(exportUrl + "/WcfApi/LoginWcfService.svc");
            ChannelFactory<ILoginWcfService> factory = new ChannelFactory<ILoginWcfService>(binding, address);
            ILoginWcfService channel = factory.CreateChannel();
            Response result = channel.LoginByCredential(email, aPICredentialsParameters);

            if (result.HasError)
                throw new Exception("enerror accord when try get token from export server, err: " + result.ErrorMessage);

            if (string.IsNullOrEmpty(result.Result))
                throw new Exception("enerror accord when try get token from export server, the token result return empty");

            tokens[GetTokenKey(tenant, email)] = result.Result;
        }
        
        private static string GetTokenKey(int tenant, string email) => tenant + ";" + email;
    }
}