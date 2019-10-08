using Logitude.Server.Tools;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FeatureWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FeatureWcfService.svc or FeatureWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FeatureWcfService : IFeatureWcfService
    {
        public List<FeatureAccessInfo> GetActiveFeaturesForUser(List<FeatureAccessInfo> featuresList, int tenant, ref Response response)
        {
            var myEmail = HttpContext.Current.User.Identity.Name;
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AzureLog.SaveLogsInStorage("( Token : " + token + " ) => this is the coming Token ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
            }
            catch (Exception)
            {
                 
            }
            if (string.IsNullOrEmpty(myEmail))
            {
                AzureLog.SaveLogsInStorage("( Tenant : " + tenant + " ) => HttpContext.Current.User.Identity.Name is null or empty ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                string token = HttpContext.Current.Request.Headers["Token"];
                AzureLog.SaveLogsInStorage("( Token : " + token + " ) => myEmail is null or empty ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                if (!string.IsNullOrEmpty(token))
                {
                    ICommonDataContext context = CommonDataContext.GetContext(0);
                    AuthenticationTokenRepository tokenRep = new AuthenticationTokenRepository(context);
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    if (authToken != null)
                    {
                        AzureLog.SaveLogsInStorage("( authToken.Email : " + authToken.Email + " ) => authToken is not null or empty ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                        myEmail = authToken.Email;
                        if (string.IsNullOrEmpty(myEmail))
                        {
                            AzureLog.SaveLogsInStorage("( Tenant : " + tenant + " ) => authToken.Email is null or empty ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                        }
                    }
                    else
                    {
                        AzureLog.SaveLogsInStorage("( Tenant : " + tenant + " ) => authToken is null ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                    }
                }
            }
            //else
            //{
                AzureLog.SaveLogsInStorage("( Tenant : " + tenant + " ) => authToken.Email is " + myEmail + " AuthOnTenant is Running", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                SecurityUtility.AuthenticationOnTenant(tenant);
            //}

            try
            {
                if (featuresList != null)
                {
                    SecurityUtility.CheckContactTableFeatures(featuresList, myEmail, tenant);

                    AzureLog.SaveLogsInStorage("( Tenant : " + tenant + " ) => CheckContactTableFeatures Done with No Problems. for Email => " + myEmail, "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                    AzureLog.SaveLogsInStorage("( Tenant : " + tenant + " ) => CheckContactTableFeatures Results => ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                    //foreach (var item in featuresList)
                    //{
                    //    AzureLog.SaveLogsInStorage("( " + item.FeatureCode + " " + item.ObjectTableName + " HasAccess => " + item.HasAccess + " ) ", "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);
                    //    item.HasAccess = true; 

                    //}
                    //SecurityUtility.CheckCustomContactTableFeatures(featuresList, HttpContext.Current.User.Identity.Name, tenant);//

                }
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                AzureLog.SaveLogsInStorage("( Tenant : " + tenant + " ) => Exception " + response.ErrorMessage , "P", DateTime.Now, "", "", 0, "", "FeatureWcfService", null);


                return null;

            }
            return featuresList;
        }

        public bool CheckOutlookVersion(string Version)
        {
            var CurrentVersionarr = Version.Split('.');
            var MinVersionarr = LogitudeSettings.MinimumOutlookVersion.Split('.');
            int Curversion = 0;
            int Minversion = 0;
            bool CheckResult = true;
            for (int i = 0; i < MinVersionarr.Length; i++)
            {
                int.TryParse(CurrentVersionarr[i], out Curversion);
                int.TryParse(MinVersionarr[i], out Minversion);
                if (Curversion < Minversion)
                {
                    CheckResult = false;
                }
            }
            return CheckResult;
        }
    }
}
