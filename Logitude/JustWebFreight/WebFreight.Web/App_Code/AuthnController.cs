using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;


namespace WebFreight.Web.App_Code
{
    public class AuthnController : ApiController
    {
        //[HttpPost]
        //[ActionName("authenticate")]

        AuthenticationController authenticationAPI = new AuthenticationController();
        public HttpResponseMessage Authenticate([FromBody] UserCredentials credentials)
        {
            try
            {
                LoginParameters loginParameters = new LoginParameters()
                {
                    Email = credentials.Email,
                    Password = credentials.Password,
                    GetToken = true
                };

                var userData = authenticationAPI.PostUserValidation(loginParameters);

                if (userData.CompanyLogins.Count > 1)
                    userData = GetUserDataByTenant(credentials, userData);


                if (userData.HasError || string.IsNullOrEmpty(userData.Token))
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, BuildErrorMessage(userData));

                return Request.CreateResponse(HttpStatusCode.OK, new { Token = userData.Token });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private UserData GetUserDataByTenant(UserCredentials credentials, UserData userData)
        {
            var companyLogin = userData.CompanyLogins.FirstOrDefault(c => c.Tenant == credentials.Tenant);
            companyLogin = companyLogin ?? userData.CompanyLogins.FirstOrDefault();

            if (companyLogin != null)
            {
                var loginParameters = new LoginParameters()
                {
                    Email = credentials.Email,
                    Password = credentials.Password,
                    CardId = companyLogin.CardId,
                    CardType = companyLogin.CardType,
                    GetToken = true
                };

                return authenticationAPI.PostLoginData(loginParameters, companyLogin.Tenant);
            }
            else
                return userData;
        }

        private static string BuildErrorMessage(UserData userData)
        {
            var errorMessage = "Invalid user email or password!";
            if (!string.IsNullOrEmpty(userData.ExceptionMessage))
                errorMessage += Environment.NewLine + userData.ExceptionMessage;
            return errorMessage;
        }
    }
}

