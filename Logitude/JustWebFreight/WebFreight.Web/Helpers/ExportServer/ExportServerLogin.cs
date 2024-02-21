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
        private static Dictionary<string, string> tokens = new Dictionary<string, string>();
        private static TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
        public readonly static string exportUrl = new SettingQuery().GetSinglePMFromCahche().ExportUrl;

        public static string GetLinkToLogin(int tenant, string email)
        {            
            int? exportTenant = tenantManagementQuery.GetSinglePM(tenant).ExportTenant;
            if (exportTenant == null)
                throw new Exception("not config in table tenant managment field field export tenant for tenant " + tenant);

            string token = GetToken(exportTenant.Value, email);
            string link = $"{exportUrl}/AmitalSSOAngular.html?token={token}&tenant={exportTenant.Value}&AmitalSSOAngular=1";
            return link;
        }

        public static string GetToken(int tenant, string email)
        {
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);            
            if (string.IsNullOrEmpty(tenantManagementPM.ExportLoginCredintial))
                throw new Exception("not config in table tenant managment field Export Login Credintial for tenant " + tenant);

            string tokenKey = GetTokenKey(tenant, email);
            if (!tokens.ContainsKey(tokenKey))
                initToken(tenant, email, tenantManagementPM.ExportLoginCredintial);

            return tokens[tokenKey];
        }

        private static void initToken(int tenant, string email, string exportLoginCredintial)
        {            
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

            tokens[GetTokenKey(tenant, email)] = result.Result;
        }
        
        private static string GetTokenKey(int tenant, string email) => tenant + ";" + email;
    }
}