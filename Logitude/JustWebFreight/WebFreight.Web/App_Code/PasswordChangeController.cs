using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WebServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.App_Code
{
    public class PasswordChangeController : ApiController
    {
        public HttpResponseMessage PostCheckUserPassword(ChangePasswordParameter changePasswordParameter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                PasswordCheckService passwordCheckService = new PasswordCheckService();
                bool result = passwordCheckService.CheckUserPassword(changePasswordParameter.CurrentPassword, changePasswordParameter.ContactId, authToken.Tenant, changePasswordParameter.Email);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage PostChangeUserPassword(ChangePasswordParameter changePasswordParameter)
        {
            try
            {
                bool result = false;
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                PasswordChangeHelper passwordChangeHelper = new PasswordChangeHelper();

                passwordChangeHelper.ValidationPassword(changePasswordParameter.Email , changePasswordParameter.NewPassword, changePasswordParameter.CurrentPassword, true);
                result = passwordChangeHelper.ChangePassword(changePasswordParameter.Email, changePasswordParameter.NewPassword, changePasswordParameter.CurrentPassword, token);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }




    //Angular Login Module
    public HttpResponseMessage PostChangePassword(ChangePasswordParameter changePasswordParameter)
        {
            try
            {
                PasswordCheckService passwordCheckService = new PasswordCheckService();
                bool result = passwordCheckService.ChangeUserPassword(changePasswordParameter.Email, changePasswordParameter.NewPassword);
               
  
             return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetResetUserPassword(string userId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ContactQuery contactQuery = new ContactQuery(tenant);
                string result = "";
                string email = contactQuery.GetContactEmailById(userId, tenant);
                PasswordCheckService passwordCheckService = new PasswordCheckService();
                var passwordCheck = passwordCheckService.ResetUserPassword(email, tenant);

                if (passwordCheck)
                {
                    result = email;
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



            
        }

       // 
        public HttpResponseMessage GetCheckIfUserIsExists(string email, int tenant, bool hasPassword, bool hasContact)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                string result = "";
                PasswordCheckService passwordCheckService = new PasswordCheckService();

                bool hasUser = passwordCheckService.CheckIfUserIsExists(email, tenant, ref hasPassword, ref hasContact);

                result = hasUser.ToString() + '@' + hasPassword.ToString();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }

        public HttpResponseMessage GetSetUserLastLogin(string password, string contactId, int tenant, string computerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                bool result = false;
                PasswordCheckService passwordCheckService = new PasswordCheckService();

                result = passwordCheckService.SetUserLastLogin(password, contactId, tenant, computerId);



                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage PostCheckPasswordUser(ChangePasswordParameter changePasswordParameter)
        {
            try
            {
                PasswordCheckService passwordCheckService = new PasswordCheckService();
                bool result = passwordCheckService.CheckUserPassword(changePasswordParameter.CurrentPassword, changePasswordParameter.ContactId, 0, changePasswordParameter.Email);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }


    public class ChangePasswordParameter
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string ContactId { get; set; }
        public string NewPassword { get; set; }

    }


}