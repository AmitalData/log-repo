using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security; 
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries; 
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;
using System.Text.RegularExpressions;
using System.Web.UI; 
using System.Web.UI.WebControls;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Counters;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using System.IO;
using Logitude.Server.Tools;
using Logitude.SystemLogs;
//using System.ServiceModel.Web;
using System.ServiceModel;
using System.Net.Http;
using WebFreight.Web.WcfApi;
using System.Net;

namespace WebFreight.Web
{

    public class APIAuthenticationController : ApiController
    {

        public ApiCredential PostApiCredintials(APICredentialsParameters Key)
        {
            APICredentialsHelper helper = new APICredentialsHelper();
            ApiCredential data;
            data = helper.CheckUserState(Key);

            if (!data.HasError)
            {
                string PrimaryhashedKey = PasswordGenerator.GetOldHashedPassword(Key.PrimaryKey);
                string SecondaryhashedKey = PasswordGenerator.GetOldHashedPassword(Key.PrimaryKey);
                //string token = Guid.NewGuid().ToString();
                //string Secondarytoken = Guid.NewGuid().ToString();

                #region apiCredintialsId
                ApiCredintialsRepository apiCredintialsRepository = new ApiCredintialsRepository();
                string apiCredintialsId = apiCredintialsRepository.GetApiCredintialsIdByHashedPrimaryAccessKey(PrimaryhashedKey);
                #endregion

                AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
                AuthenticationToken Primaryauthentication = new AuthenticationToken() { APICredentialID = apiCredintialsId, CreateDate = DateTime.Now, Email = "system@tenant" + data.Tenant + ".com", Password = PrimaryhashedKey, Token = data.Token, Tenant = data.Tenant, APIToken = true };
                //AuthenticationToken Secondaryauthentication = new AuthenticationToken() { CreateDate = DateTime.Now, Email = "system@tenant" + data.Tenant + ".com", Password = SecondaryhashedKey, Token = Secondarytoken, Tenant = data.Tenant, APIToken = true };
                authenticationTokenRepository.Add(Primaryauthentication);
                //authenticationTokenRepository.Add(Secondaryauthentication);
                authenticationTokenRepository.SubmitChanges();
                //data.Token = token;
            }
            return data;
        }


        public HttpResponseMessage GetTokenByCredentialKey(string email, int tenant)
        {
            try
            {
                string credentialKey = HttpContext.Current.Request.Headers["CredentialKey"];
                APICredentialsParameters apiCredentialsParam = new APICredentialsParameters() { PrimaryKey = credentialKey, SecondaryKey = credentialKey, Tenant = tenant };
                AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(apiCredentialsParam.Tenant);
                APICredentialsHelper helper = new APICredentialsHelper();
                ApiCredential data = helper.CheckUserState(apiCredentialsParam);
                
                if (!data.HasError)
                {
                    string PrimaryhashedKey = PasswordGenerator.GetOldHashedPassword(credentialKey);
                
                    #region apiCredintialsId
                    ApiCredintialsRepository apiCredintialsRepository = new ApiCredintialsRepository();
                    string apiCredintialsId = apiCredintialsRepository.GetApiCredintialsIdByHashedPrimaryAccessKey(PrimaryhashedKey);
                    #endregion

                    AuthenticationToken Primaryauthentication = new AuthenticationToken() {APICredentialID = apiCredintialsId, CreateDate = DateTime.Now, Email = email, Password = PrimaryhashedKey, Token = data.Token, Tenant = apiCredentialsParam.Tenant, APIToken = true };
                    authenticationTokenRepository.Add(Primaryauthentication);
                    authenticationTokenRepository.SubmitChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, data.Token);
                }
                else
                {
                    APIException apiException = new APIException()
                    {
                        ErrorType = "Exception",
                        ErrorMessage = "Invalid api credentials key",
                        ShortErrorMessage = "Invalid api credentials key",
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }

        }

        public HttpResponseMessage GetApiSettingsParametersByTanentSettings(string token)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);           
                ExternalAPIHelper externalAPIHelper = new ExternalAPIHelper(authToken.Tenant);
                ExternalAPIResponseParameters responseParameters = externalAPIHelper.GetExternalAPIResponseParameters();

                return Request.CreateResponse(HttpStatusCode.OK, responseParameters);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }

}
